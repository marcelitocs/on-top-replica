using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Native Point structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NPoint {
        public int X, Y;

        /// <summary>
        /// Creates a new point.
        /// </summary>
        public NPoint(int x, int y) {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new point from another point.
        /// </summary>
        public NPoint(NPoint copy) {
            X = copy.X;
            Y = copy.Y;
        }

        /// <summary>
        /// Creates a native point from a framework point.
        /// </summary>
        public static NPoint FromPoint(System.Drawing.Point point) {
            return new NPoint(point.X, point.Y);
        }

        /// <summary>
        /// Converts the native point to a framework point.
        /// </summary>
        public System.Drawing.Point ToPoint() {
            return new System.Drawing.Point(X, Y);
        }

        public override string ToString() {
            return "{" + X + "," + Y + "}";
        }
    }
}
