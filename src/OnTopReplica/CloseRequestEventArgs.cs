using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OnTopReplica {
    /// <summary>
    /// Event arguments for a close request.
    /// </summary>
	public class CloseRequestEventArgs : EventArgs {

        /// <summary>
        /// Gets or sets the last window handle.
        /// </summary>
		public WindowHandle LastWindowHandle { get; set; }

        /// <summary>
        /// Gets or sets the last region.
        /// </summary>
        public Rectangle? LastRegion { get; set; }

	}
}
