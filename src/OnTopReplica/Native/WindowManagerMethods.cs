using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common Win32 Window Manager native methods.
    /// </summary>
    static class WindowManagerMethods {

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working).
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Retrieves a handle to the child window at the specified point.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr RealChildWindowFromPoint(IntPtr parent, NPoint point);

        /// <summary>
        /// An application-defined callback function used with the EnumWindows or EnumDesktopWindows function.
        /// </summary>
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        /// Enumerates all top-level windows on the screen by passing the handle to each window, in turn, to an application-defined callback function.
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// Enumerates the child windows that belong to the specified parent window by passing the handle to each child window, in turn, to an application-defined callback function.
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// Determines the visibility state of the specified window.
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// Retrieves a handle to the desktop window.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hwnd, ref NPoint point);

        /// <summary>
        /// Converts a point in client coordinates of a window to screen coordinates.
        /// </summary>
        /// <param name="hwnd">Handle to the window of the original point.</param>
        /// <param name="clientPoint">Point expressed in client coordinates.</param>
        /// <returns>Point expressed in screen coordinates.</returns>
        public static NPoint ClientToScreen(IntPtr hwnd, NPoint clientPoint) {
            NPoint localCopy = new NPoint(clientPoint);

            if (ClientToScreen(hwnd, ref localCopy))
                return localCopy;
            else
                return new NPoint();
        }

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hwnd, ref NPoint point);

        /// <summary>
        /// Converts a point in screen coordinates in client coordinates relative to a window.
        /// </summary>
        /// <param name="hwnd">Handle of the window whose client coordinate system should be used.</param>
        /// <param name="screenPoint">Point expressed in screen coordinates.</param>
        /// <returns>Point expressed in client coordinates.</returns>
        public static NPoint ScreenToClient(IntPtr hwnd, NPoint screenPoint) {
            NPoint localCopy = new NPoint(screenPoint);

            if (ScreenToClient(hwnd, ref localCopy))
                return localCopy;
            else
                return new NPoint();
        }

        /// <summary>
        /// Retrieves a handle to the parent or owner of the specified window.
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// Changes the parent window of the specified child window.
        /// </summary>
        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndParent);

        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window.
        /// </summary>
        [DllImport("user32.dll", SetLastError = false)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// Window retrieval flags.
        /// </summary>
        public enum GetWindowMode : uint {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        /// <summary>
        /// Retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hwnd, GetWindowMode mode);

        /// <summary>
        /// Checks whether a window is a top-level window (has no owner nor parent window).
        /// </summary>
        /// <param name="hwnd">Handle to the window to check.</param>
        public static bool IsTopLevel(IntPtr hwnd) {
            bool hasParent = WindowManagerMethods.GetParent(hwnd).ToInt64() != 0;
            bool hasOwner = WindowManagerMethods.GetWindow(hwnd, WindowManagerMethods.GetWindowMode.GW_OWNER).ToInt64() != 0;

            return (!hasParent && !hasOwner);
        }

        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);
    }
}
