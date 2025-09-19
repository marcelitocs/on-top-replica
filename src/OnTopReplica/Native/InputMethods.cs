using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {

    /// <summary>
    /// P/Invoke wrapper for input methods.
    /// </summary>
    static class InputMethods {

        [DllImport("user32.dll")]
        static extern short GetKeyState(VirtualKeyState nVirtKey);

        const int KeyToggled = 0x1;

        const int KeyPressed = 0x8000;

        /// <summary>
        /// Gets whether a key is currently pressed.
        /// </summary>
        /// <param name="virtKey">Virtual key to check.</param>
        /// <returns>True if the key is pressed.</returns>
        public static bool IsKeyPressed(VirtualKeyState virtKey) {
            return (GetKeyState(virtKey) & KeyPressed) != 0;
        }

        /// <summary>
        /// Gets whether a key is currently toggled.
        /// </summary>
        /// <param name="virtKey">Virtual key to check.</param>
        /// <returns>True if the key is toggled.</returns>
        public static bool IsKeyToggled(VirtualKeyState virtKey) {
            return (GetKeyState(virtKey) & KeyToggled) != 0;
        }

    }
}
