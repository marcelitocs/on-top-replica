using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace OnTopReplica.Native {

    /// <summary>A native Rectangle Structure.</summary>
    [StructLayout(LayoutKind.Sequential)]
    struct NRectangle {
        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        public NRectangle(int left, int top, int right, int bottom) {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Creates a new rectangle from a framework rectangle.
        /// </summary>
        public NRectangle(System.Drawing.Rectangle rect) {
            Left = rect.X;
            Top = rect.Y;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int Width {
            get {
                return Right - Left;
            }
        }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int Height {
            get {
                return Bottom - Top;
            }
        }

        /// <summary>
        /// Converts the native rectangle to a framework rectangle.
        /// </summary>
        public System.Drawing.Rectangle ToRectangle() {
            return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
        }

        /// <summary>
        /// Gets the size of the rectangle.
        /// </summary>
        public System.Drawing.Size Size {
            get {
                return new System.Drawing.Size(Width, Height);
            }
        }

        public override string ToString() {
            return string.Format("{{{0},{1},{2},{3}}}", Left, Top, Right, Bottom);
        }

    }

    /// <summary>
    /// Extension methods for native rectangles.
    /// </summary>
    static class NRectangleExtensions {

        /// <summary>
        /// Converts a framework size to a native rectangle.
        /// </summary>
        public static NRectangle ToNativeRectangle(this Size size) {
            return new NRectangle(0, 0, size.Width, size.Height);
        }

    }

}
