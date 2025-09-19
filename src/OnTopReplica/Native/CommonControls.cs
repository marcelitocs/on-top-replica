using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {

    /// <summary>
    /// P/Invoke wrapper for the Common Controls library (comctl32.dll).
    /// </summary>
    public static class CommonControls {

        [DllImport("comctl32.dll", EntryPoint = "InitCommonControlsEx", CallingConvention = CallingConvention.StdCall)]
        static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX iccex);

        const int ICC_STANDARD_CLASSES = 0x00004000;
        const int ICC_WIN95_CLASSES = 0x000000FF;

        /// <summary>
        /// Initializes standard common controls.
        /// </summary>
        /// <returns>True if successful.</returns>
        public static bool InitStandard() {
            INITCOMMONCONTROLSEX ex = new INITCOMMONCONTROLSEX();
            ex.dwSize = 8;
            ex.dwICC = ICC_STANDARD_CLASSES | ICC_WIN95_CLASSES;

            return InitCommonControlsEx(ref ex);
        }

    }

    /// <summary>
    /// Structure for InitCommonControlsEx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct INITCOMMONCONTROLSEX {
        public int dwSize;
        public int dwICC;
    }

}
