using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnTopReplica.MessagePumpProcessors;
using OnTopReplica.WindowSeekers;

namespace OnTopReplica.SidePanels {
    /// <summary>
    /// Side panel for group switch mode.
    /// </summary>
    partial class GroupSwitchPanel : SidePanel {
        /// <summary>
        /// Creates a new instance of the group switch panel.
        /// </summary>
        public GroupSwitchPanel() {
            InitializeComponent();

            LocalizePanel();
        }

        private void LocalizePanel() {
            groupBox1.Text = Strings.GroupSwitchModeTitle;
            buttonEnable.Text = Strings.GroupSwitchModeEnableButton;
            buttonCancel.Text = Strings.GroupSwitchModeDisableButton;
        }

        /// <summary>
        /// Gets the title of the panel.
        /// </summary>
        public override string Title {
            get {
                return Strings.MenuGroupSwitch;
            }
        }

        /// <summary>
        /// Overridden. Loads the window list when the panel is first shown.
        /// </summary>
        /// <param name="form">The main form.</param>
        public override void OnFirstShown(MainForm form) {
            base.OnFirstShown(form);

            LoadWindowList();

            labelStatus.Text = (ParentMainForm.MessagePumpManager.Get<GroupSwitchManager>().IsActive) ?
                Strings.GroupSwitchModeStatusEnabled :
                Strings.GroupSwitchModeStatusDisabled;
        }

        private void LoadWindowList() {
            var manager = new TaskWindowSeeker {
                SkipNotVisibleWindows = true
            };
            manager.Refresh();

            var imageList = new ImageList();
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            foreach (var w in manager.Windows) {
                var item = new ListViewItem(w.Title) {
                    Tag = w
                };

                if (w.Icon != null) {
                    imageList.Images.Add(w.Icon);
                    item.ImageIndex = imageList.Images.Count - 1;
                }

                listWindows.Items.Add(item);
            }
            listWindows.SmallImageList = imageList;
        }

        /// <summary>
        /// Overridden. Enables or disables group switch mode when the panel is closing.
        /// </summary>
        /// <param name="form">The main form.</param>
        public override void OnClosing(MainForm form) {
            base.OnClosing(form);

            if (_enableOnClose && listWindows.SelectedItems.Count > 0) {
                List<WindowHandle> ret = new List<WindowHandle>();
                foreach (ListViewItem i in listWindows.SelectedItems) {
                    ret.Add((WindowHandle)i.Tag);
                }

                form.SetThumbnailGroup(ret);
            }
            else {
                form.MessagePumpManager.Get<GroupSwitchManager>().Disable();
            }
        }

        bool _enableOnClose = false;

        private void Enable_click(object sender, EventArgs e) {
            _enableOnClose = true;
            OnRequestClosing();
        }

        private void Cancel_click(object sender, EventArgs e) {
            OnRequestClosing();
        }

    }

}
