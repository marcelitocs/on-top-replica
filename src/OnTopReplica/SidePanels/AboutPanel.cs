using OnTopReplica.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsFormsAero.Dwm;

namespace OnTopReplica.SidePanels {
    /// <summary>
    /// About panel.
    /// </summary>
    partial class AboutPanel : SidePanel {

        /// <summary>
        /// Creates a new instance of the about panel.
        /// </summary>
        public AboutPanel() {
            InitializeComponent();

            //Display version number
            labelVersion.Text = string.Format(Strings.AboutVersion, Application.ProductVersion);
        }

        /// <summary>
        /// Gets the title of the panel.
        /// </summary>
        public override string Title {
            get {
                return Strings.AboutTitle;
            }
        }

        /// <summary>
        /// Gets the glass margins of the panel.
        /// </summary>
        public override Padding GlassMargins {
            get {
                return new Padding(0, 0, 0, labelVersion.Height);
            }
        }

    }
}
