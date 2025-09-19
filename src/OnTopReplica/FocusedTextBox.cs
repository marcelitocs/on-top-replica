using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsAero;
using System.Windows.Forms;

namespace OnTopReplica {

    /// <summary>
    /// A text box that raises events when the user confirms or aborts input.
    /// </summary>
	class FocusedTextBox : System.Windows.Forms.TextBox {

        /// <summary>
        /// Overridden.
        /// </summary>
		protected override bool IsInputChar(char charCode) {
			if (charCode == '\n' || charCode == '\r')
				return true;

			return base.IsInputChar(charCode);
		}

        /// <summary>
        /// Overridden. Raises the ConfirmInput or AbortInput event when the user presses Enter or Escape.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
		protected override void OnKeyUp(KeyEventArgs e) {
			if (e.KeyCode == Keys.Return) {
                if(!string.IsNullOrEmpty(Text))
				    OnConfirmInput();

				e.Handled = true;
                e.SuppressKeyPress = true;
			}
            else if (e.KeyCode == Keys.Escape) {
                OnAbortInput();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            base.OnKeyUp(e);
		}

        //List of characters to ignore on KeyPress events (because they generate bell rings)
        readonly char[] IgnoreChars = new char[] {
            (char)27, (char)13
        };

        /// <summary>
        /// Overridden.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e) {
            if (IgnoreChars.Contains(e.KeyChar)) {
                e.Handled = true;
            }

            base.OnKeyPress(e);
        }

        /// <summary>
        /// Fired when the user confirms input by pressing Enter.
        /// </summary>
		public event EventHandler ConfirmInput;

        /// <summary>
        /// Raises the ConfirmInput event.
        /// </summary>
		protected virtual void OnConfirmInput() {
            var evt = ConfirmInput;
            if (evt != null)
                evt(this, EventArgs.Empty);
		}

        /// <summary>
        /// Fired when the user aborts input by pressing Escape.
        /// </summary>
        public event EventHandler AbortInput;

        /// <summary>
        /// Raises the AbortInput event.
        /// </summary>
        protected virtual void OnAbortInput() {
            var evt = AbortInput;
            if (evt != null)
                evt(this, EventArgs.Empty);
        }

	}

}
