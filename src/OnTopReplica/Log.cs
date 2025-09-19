using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OnTopReplica {
    /// <summary>
    /// Simple logger.
    /// </summary>
    static class Log {

        const string LogFileName = "lastrun.log.txt";
        const string ConflictLogFileName = "run-{0}.log.txt";

        private readonly static StreamWriter Writer;

        static Log() {
            try {
                var filepath = Path.Combine(AppPaths.PrivateRoamingFolderPath, LogFileName);
                Writer = new StreamWriter(new FileStream(filepath, FileMode.Create));
                Writer.AutoFlush = true;
            }
            catch (Exception) {
                try {
                    var filepath = Path.Combine(AppPaths.PrivateRoamingFolderPath, string.Format(ConflictLogFileName, System.Diagnostics.Process.GetCurrentProcess().Id));
                    Writer = new StreamWriter(new FileStream(filepath, FileMode.Create));
                    Writer.AutoFlush = true;
                }
                catch (Exception) {
                    //No fallback logging possible
                    Writer = null;
                }
            }
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Write(string message) {
            WriteLine(message);
        }

        /// <summary>
        /// Writes a formatted message to the log.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument.</param>
        public static void Write(string format, object arg0) {
            WriteLine(string.Format(format, arg0));
        }

        /// <summary>
        /// Writes a formatted message to the log.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        public static void Write(string format, object arg0, object arg1) {
            WriteLine(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// Writes a formatted message to the log.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        public static void Write(string format, params object[] args) {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// Writes a message with details to the log.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        public static void WriteDetails(string caption, string format, params object[] args) {
            WriteLines(caption, string.Format(format, args));
        }

        /// <summary>
        /// Writes an exception to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception to write.</param>
        public static void WriteException(string message, Exception exception) {
            if (exception != null) {
                WriteLines(message, exception.ToString());
            }
            else {
                WriteLines(message, "(No exception data.)");
            }
        }

        private static void WriteLine(string message) {
            var s = string.Format("{0,-8:HH:mm:ss} {1}", DateTime.Now, message);
            AddToQueue(s);

            if (Writer != null) {
                Writer.WriteLine(s);
            }
        }

        private static void WriteLines(params string[] messages) {
            if (messages.Length <= 0)
                return;

            var sb = new StringBuilder();
            sb.AppendFormat("{0,-8:HH:mm:ss} {1}", DateTime.Now, messages[0]);
            for (int i = 1; i < messages.Length; ++i) {
                sb.AppendLine();
                sb.AppendFormat("         {0}", messages[i]);
            }

            AddToQueue(sb.ToString());

            if (Writer != null) {
                Writer.WriteLine(sb.ToString());
            }
        }

        const int MaxQueueCapacity = 30;

        private static Queue<string> _entriesQueue = new Queue<string>(MaxQueueCapacity);

        private static void AddToQueue(string entry){
            _entriesQueue.Enqueue(entry);

            while(_entriesQueue.Count > MaxQueueCapacity){
                _entriesQueue.Dequeue();
            }
        }

        /// <summary>
        /// Gets the queue of log entries.
        /// </summary>
        public static IEnumerable<string> Queue {
            get {
                return _entriesQueue;
            }
        }

    }
}
