using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common methods for Win32 messaging.
    /// </summary>
    static class MessagingMethods {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Flags for SendMessageTimeout.
        /// </summary>
        [Flags]
        public enum SendMessageTimeoutFlags : uint {
            /// <summary>
            /// The calling thread is not prevented from processing other requests until the window that is receiving the message processes the message.
            /// </summary>
            AbortIfHung = 2,
            /// <summary>
            /// The function returns without waiting for the time-out period to elapse if the receiving thread appears to be "hung."
            /// </summary>
            Block = 1,
            /// <summary>
            /// The calling thread is not prevented from processing other requests until the window that is receiving the message processes the message.
            /// </summary>
            Normal = 0
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint message, IntPtr wparam, IntPtr lparam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = false)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Creates an LParam for a Windows message from two 16-bit values.
        /// </summary>
        /// <param name="LoWord">The low-order word.</param>
        /// <param name="HiWord">The high-order word.</param>
        /// <returns>The LParam.</returns>
        public static IntPtr MakeLParam(int LoWord, int HiWord) {
            return new IntPtr((HiWord << 16) | (LoWord & 0xffff));
        }

    }
}
