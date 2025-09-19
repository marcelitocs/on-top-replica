using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.StartupOptions {
    /// <summary>
    /// Form that displays command line parsing errors or help.
    /// </summary>
    public partial class CommandLineReportForm : Form {

        /// <summary>
        /// Creates a new instance of the command line report form.
        /// </summary>
        /// <param name="status">The status of the command line parsing.</param>
        /// <param name="message">The message to display.</param>
        public CommandLineReportForm(CliStatus status, string message) {
            InitializeComponent();

            switch (status) {
                case CliStatus.Information:
                    labelInstruction.Text = "Command line help";
                    break;

                case CliStatus.Error:
                    labelInstruction.Text = "Command line parsing error";
                    break;
            }

            txtDescription.Text = message;

            txtCliArgs.Text = Environment.CommandLine;
        }

    }
}
