using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    
    /// <summary>
    /// Window seeker that attempts to locate a window to restore (by class, title and ID).
    /// </summary>
    class RestoreWindowSeeker : PointBasedWindowSeeker {

        /// <summary>
        /// Creates a new instance of the seeker.
        /// </summary>
        /// <param name="handle">The handle of the window to restore.</param>
        /// <param name="title">The title of the window to restore.</param>
        /// <param name="className">The class name of the window to restore.</param>
        public RestoreWindowSeeker(IntPtr handle, string title, string className){
            Handle = handle;
            Title = title;
            Class = className;
        }

        /// <summary>
        /// Gets the handle of the window to restore.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets the title of the window to restore.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the class name of the window to restore.
        /// </summary>
        public string Class { get; private set; }

        protected override int EvaluatePoints(WindowHandle handle) {
            if (!WindowManagerMethods.IsTopLevel(handle.Handle)) {
                return -1;
            }

            int points = 0;

            //Class exact match
            if (!string.IsNullOrEmpty(Class)) {
                string wndClass = handle.Class;
                if (wndClass.Equals(Class, StringComparison.InvariantCulture)){
                    points += 10;
                }
            }

            //Title match (may not be exact, but let's try)
            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(handle.Title)) {
                if (handle.Title.StartsWith(Title, StringComparison.InvariantCultureIgnoreCase)) {
                    points += 5;
                }
                if (handle.Title.Equals(Title, StringComparison.InvariantCultureIgnoreCase)) {
                    points += 10;
                }
            }

            //Handle match (will probably not work, but anyhow)
            if (Handle != IntPtr.Zero) {
                if (Handle == handle.Handle) {
                    points += 10;
                }
            }

            return points;
        }
    }

}
