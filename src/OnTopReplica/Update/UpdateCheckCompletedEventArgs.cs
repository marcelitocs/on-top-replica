using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Update {
    /// <summary>
    /// Event arguments for the update check completed event.
    /// </summary>
    class UpdateCheckCompletedEventArgs : EventArgs {

        /// <summary>
        /// Gets or sets the update information.
        /// </summary>
        public UpdateInformation Information { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the update check was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error that occurred during the update check.
        /// </summary>
        public Exception Error { get; set; }

    }
}
