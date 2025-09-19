using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {

    /// <summary>
    /// Interface for message pump processors.
    /// </summary>
    interface IMessagePumpProcessor : IDisposable {

        /// <summary>
        /// Initializes the processor.
        /// </summary>
        /// <param name="form">The main form.</param>
        void Initialize(MainForm form);

        /// <summary>
        /// Processes a Windows message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>True if the message has been handled and should not be processed further.</returns>
        bool Process(ref Message msg);

    }
}
