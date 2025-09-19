using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Native {
    /// <summary>
    /// Native Win32 sizing codes (used by WM_SIZING message).
    /// </summary>
    static class WMSZ {
        /// <summary>
        /// The left edge of the window is being resized.
        /// </summary>
        public const int LEFT = 1;
        /// <summary>
        /// The right edge of the window is being resized.
        /// </summary>
        public const int RIGHT = 2;
        /// <summary>
        /// The top edge of the window is being resized.
        /// </summary>
        public const int TOP = 3;
        /// <summary>
        /// The bottom edge of the window is being resized.
        /// </summary>
        public const int BOTTOM = 6;
    }
}
