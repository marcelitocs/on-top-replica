using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;
using OnTopReplica.StartupOptions;

namespace OnTopReplica
{
    /// <summary>
    /// Represents a stored scenario that can be launched.
    /// </summary>
    public class StoredScenario
    {
        /// <summary>
        /// Gets or sets the name of the scenario.
        /// </summary>
        public string Name { get; set; }

        #region Window matching

        /// <summary>
        /// Gets or sets the window title to clone.
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Gets or sets the window class to clone.
        /// </summary>
        public string WindowClass { get; set; }

        #endregion

        #region Visuals

        /// <summary>
        /// Gets or sets the region to clone.
        /// </summary>
        public ThumbnailRegion Region { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the window.
        /// </summary>
        public byte Opacity { get; set; }

        /// <summary>
        /// Gets or sets whether to disable the window chrome.
        /// </summary>
        public bool IsChromeVisible { get; set; }

        #endregion

        /// <summary>
        /// Default constructor for XML serialization.
        /// </summary>
        public StoredScenario()
        {
            Opacity = 255;
            IsChromeVisible = true;
        }

        /// <summary>
        /// Converts the stored scenario to a startup options object.
        /// </summary>
        public Options ToOptions()
        {
            var options = new Options
            {
                WindowTitle = this.WindowTitle,
                WindowClass = this.WindowClass,
                Region = this.Region,
                Opacity = this.Opacity,
                DisableChrome = !this.IsChromeVisible
            };
            return options;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
