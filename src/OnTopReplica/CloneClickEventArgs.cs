using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OnTopReplica {
    /// <summary>
    /// EventArgs structure for clicks on a cloned window.
    /// </summary>
	public class CloneClickEventArgs : EventArgs {

        /// <summary>
        /// Gets or sets the location of the click in client coordinates.
        /// </summary>
		public Point ClientClickLocation { get; set; }

        /// <summary>
        /// Gets or sets whether the click was a double click.
        /// </summary>
		public bool IsDoubleClick { get; set; }

        /// <summary>
        /// Gets or sets the mouse buttons that were pressed.
        /// </summary>
        public MouseButtons Buttons { get; set; }

        /// <summary>
        /// Creates a new instance of the CloneClickEventArgs class.
        /// </summary>
        /// <param name="location">The location of the click in client coordinates.</param>
        /// <param name="buttons">The mouse buttons that were pressed.</param>
		public CloneClickEventArgs(Point location, MouseButtons buttons) {
			ClientClickLocation = location;
            Buttons = buttons;
			IsDoubleClick = false;
		}

        /// <summary>
        /// Creates a new instance of the CloneClickEventArgs class.
        /// </summary>
        /// <param name="location">The location of the click in client coordinates.</param>
        /// <param name="buttons">The mouse buttons that were pressed.</param>
        /// <param name="doubleClick">Whether the click was a double click.</param>
		public CloneClickEventArgs(Point location, MouseButtons buttons, bool doubleClick) {
			ClientClickLocation = location;
            Buttons = buttons;
			IsDoubleClick = doubleClick;
		}

	}
}
