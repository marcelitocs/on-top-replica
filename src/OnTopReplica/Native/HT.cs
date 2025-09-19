using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Native {
    /// <summary>
    /// Native Win32 Hit Testing codes.
    /// </summary>
    static class HT {
        /// <summary>
        /// In a client area.
        /// </summary>
        public const int TRANSPARENT = -1;
        /// <summary>
        /// In a client area.
        /// </summary>
        public const int CLIENT = 1;
        /// <summary>
        /// In a title bar.
        /// </summary>
        public const int CAPTION = 2;
    }
}
