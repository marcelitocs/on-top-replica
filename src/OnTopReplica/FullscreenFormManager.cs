using OnTopReplica.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {
    /// <summary>
    /// Manages the fullscreen state of the main form.
    /// </summary>
    class FullscreenFormManager {

        private readonly MainForm _mainForm;

        /// <summary>
        /// Creates a new instance of the fullscreen manager.
        /// </summary>
        /// <param name="form">The main form to manage.</param>
        public FullscreenFormManager(MainForm form) {
            _mainForm = form;
            IsFullscreen = false;
        }

        Point _preFullscreenLocation;
        Size _preFullscreenSize;
        FormBorderStyle _preFullscreenBorderStyle;

        /// <summary>
        /// Gets whether the form is currently in fullscreen mode.
        /// </summary>
        public bool IsFullscreen {
            get;
            private set;
        }

        /// <summary>
        /// Switches the form to fullscreen mode.
        /// </summary>
        public void SwitchFullscreen() {
            SwitchFullscreen(Settings.Default.GetFullscreenMode());
        }

        /// <summary>
        /// Switches the form to a specific fullscreen mode.
        /// </summary>
        /// <param name="mode">The fullscreen mode to switch to.</param>
        public void SwitchFullscreen(FullscreenMode mode) {
            if (IsFullscreen) {
                MoveToFullscreenMode(mode);
                return;
            }

            if (!_mainForm.ThumbnailPanel.IsShowingThumbnail)
                return;

            //On switch, always hide side panels
            _mainForm.CloseSidePanel();

            //Store state
            _preFullscreenLocation = _mainForm.Location;
            _preFullscreenSize = _mainForm.ClientSize;
            _preFullscreenBorderStyle = _mainForm.FormBorderStyle;

            //Change state to fullscreen
            _mainForm.FormBorderStyle = FormBorderStyle.None;
            MoveToFullscreenMode(mode);

            CommonCompleteSwitch(true);
        }

        private void MoveToFullscreenMode(FullscreenMode mode) {
            var screens = Screen.AllScreens;

            var currentScreen = Screen.FromControl(_mainForm);
            Size size = _mainForm.Size;
            Point location = _mainForm.Location;

            switch (mode) {
                case FullscreenMode.Standard:
                default:
                    size = currentScreen.WorkingArea.Size;
                    location = currentScreen.WorkingArea.Location;
                    break;

                case FullscreenMode.Fullscreen:
                    size = currentScreen.Bounds.Size;
                    location = currentScreen.Bounds.Location;
                    break;

                case FullscreenMode.AllScreens:
                    size = SystemInformation.VirtualScreen.Size;
                    location = SystemInformation.VirtualScreen.Location;
                    break;
            }

            _mainForm.Size = size;
            _mainForm.Location = location;
        }

        /// <summary>
        /// Switches the form back from fullscreen mode to its previous state.
        /// </summary>
        public void SwitchBack() {
            if (!IsFullscreen)
                return;

            //Restore state
            _mainForm.FormBorderStyle = _preFullscreenBorderStyle;
            _mainForm.Location = _preFullscreenLocation;
            _mainForm.ClientSize = _preFullscreenSize;
            _mainForm.RefreshAspectRatio();

            CommonCompleteSwitch(false);
        }

        private void CommonCompleteSwitch(bool enabled) {
            //UI stuff switching
            _mainForm.GlassMargins = (!enabled) ? new Padding(-1) : Padding.Empty;
            _mainForm.TopMost = !enabled;

            IsFullscreen = enabled;

            Program.Platform.OnFormStateChange(_mainForm);
        }

        /// <summary>
        /// Toggles the fullscreen mode.
        /// </summary>
        public void Toggle() {
            if (IsFullscreen)
                SwitchBack();
            else
                SwitchFullscreen(Settings.Default.GetFullscreenMode());
        }

    }
}
