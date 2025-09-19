using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.MessagePumpProcessors {
    /// <summary>
    /// Base class for message pump processors.
    /// </summary>
    abstract class BaseMessagePumpProcessor : IMessagePumpProcessor {

        /// <summary>
        /// Gets the main form instance.
        /// </summary>
        protected MainForm Form { get; private set; }

        #region IMessagePumpProcessor Members

        /// <summary>
        /// Initializes the processor.
        /// </summary>
        /// <param name="form">Main form.</param>
        public virtual void Initialize(MainForm form) {
            Form = form;
        }

        /// <summary>
        /// Processes a Windows message.
        /// </summary>
        /// <param name="msg">Message to process.</param>
        /// <returns>True if the message has been handled and should not be processed further.</returns>
        public abstract bool Process(ref Message msg);

        #endregion

        /// <summary>
        /// Called when the processor is shut down.
        /// </summary>
        protected abstract void Shutdown();

        bool _isDisposed = false;

        #region IDisposable Members

        /// <summary>
        /// Disposes the processor.
        /// </summary>
        public void Dispose() {
            if (_isDisposed)
                return;

            Shutdown();

            _isDisposed = true;
        }

        #endregion

    }
}
