using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {

    /// <summary>
    /// P/Invoke wrapper for Windows 7 specific methods.
    /// </summary>
    static class WindowsSevenMethods {

        /// <summary>
        /// Specifies a unique application-defined Application User Model ID (AppUserModelID) that identifies the current process to the taskbar.
        /// </summary>
        /// <param name="appId">The AppUserModelID to assign to the current process.</param>
        [DllImport("shell32.dll")]
        internal static extern void SetCurrentProcessExplicitAppUserModelID(
            [MarshalAs(UnmanagedType.LPWStr)] string appId);

        /// <summary>
        /// Retrieves the application-defined Application User Model ID (AppUserModelID) for the current process.
        /// </summary>
        /// <param name="appId">A pointer to a string that receives the AppUserModelID assigned to the process.</param>
        [DllImport("shell32.dll")]
        internal static extern void GetCurrentProcessExplicitAppUserModelID(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] out string appId);

    }

}
