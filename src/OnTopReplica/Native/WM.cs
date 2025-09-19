using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Native {
    /// <summary>
    /// Native Windows Message codes.
    /// </summary>
    static class WM {
        /// <summary>
        /// Sent to a window to retrieve a handle to the large or small icon associated with a window.
        /// </summary>
        public const int GETICON = 0x7f;
        /// <summary>
        /// Sent to a window that the user is resizing.
        /// </summary>
        public const int SIZING = 0x214;
        /// <summary>
        /// Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate.
        /// </summary>
        public const int NCHITTEST = 0x84;
        /// <summary>
        /// Sent to a window when its frame must be painted.
        /// </summary>
        public const int NCPAINT = 0x0085;
        /// <summary>
        /// Posted when the user presses the left mouse button while the cursor is in the client area of a window.
        /// </summary>
        public const int LBUTTONDOWN = 0x0201;
        /// <summary>
        /// Posted when the user releases the left mouse button while the cursor is in the client area of a window.
        /// </summary>
        public const int LBUTTONUP = 0x0202;
        /// <summary>
        /// Posted when the user double-clicks the left mouse button while the cursor is in the client area of a window.
        /// </summary>
        public const int LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// Posted when the user presses the right mouse button while the cursor is in the client area of a window.
        /// </summary>
        public const int RBUTTONDOWN = 0x0204;
        /// <summary>
        /// Posted when the user releases the right mouse button while the cursor is in the client area of a window.
        /// </summary>
        public const int RBUTTONUP = 0x0205;
        /// <summary>
        /// Posted when the user double-clicks the right mouse button while the cursor is in the client area of a window.
        /// </summary>
        public const int RBUTTONDBLCLK = 0x0206;
        /// <summary>
        /// Posted when the user releases the left mouse button while the cursor is in a nonclient area of a window.
        /// </summary>
        public const int NCLBUTTONUP = 0x00A2;
        /// <summary>
        /// Posted when the user presses the left mouse button while the cursor is in a nonclient area of a window.
        /// </summary>
        public const int NCLBUTTONDOWN = 0x00A1;
        /// <summary>
        /// Posted when the user double-clicks the left mouse button while the cursor is in a nonclient area of a window.
        /// </summary>
        public const int NCLBUTTONDBLCLK = 0x00A3;
        /// <summary>
        /// Posted when the user releases the right mouse button while the cursor is in a nonclient area of a window.
        /// </summary>
        public const int NCRBUTTONUP = 0x00A5;
        /// <summary>
        /// Posted to a window when the cursor leaves the nonclient area of the window.
        /// </summary>
        public const int NCMOUSELEAVE = 0x02A2;
        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) or when the user chooses the maximize button, minimize button, restore button, or close button.
        /// </summary>
        public const int SYSCOMMAND = 0x0112;
        /// <summary>
        /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.
        /// </summary>
        public const int GETTEXT = 0x000D;
        /// <summary>
        /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.
        /// </summary>
        public const int GETTEXTLENGTH = 0x000E;
    }
}
