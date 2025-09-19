using OnTopReplica.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnTopReplica.MessagePumpProcessors {

    /// <summary>
    /// Automatically clones windows that are flashing.
    /// </summary>
    class FlashCloner : BaseMessagePumpProcessor {

        /// <summary>
        /// Processes a Windows message.
        /// </summary>
        /// <param name="msg">Message to process.</param>
        /// <returns>True if the message has been handled and should not be processed further.</returns>
        public override bool Process(ref System.Windows.Forms.Message msg) {
            if (false &&
                msg.Msg == HookMethods.WM_SHELLHOOKMESSAGE) {
                int hookCode = msg.WParam.ToInt32();

                if (hookCode == HookMethods.HSHELL_FLASH) {
                    IntPtr flashHandle = msg.LParam;

                    Form.SetThumbnail(new WindowHandle(flashHandle), null);
                }
            }

            return false;
        }

        /// <summary>
        /// Called when the processor is shut down.
        /// </summary>
        protected override void Shutdown() {
            
        }

    }

}
