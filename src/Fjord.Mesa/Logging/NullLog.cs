using System;
using System.Text;

namespace Fjord.Mesa.Logging
{
    /// <summary>
    /// The default logger until one is set.
    /// </summary>
    public class NullLog : ILog, ILog<NullLog>
    {
        public void InitializeFor(string loggerName)
        {
        }

        private void WriteLog(string prefix, string message, params object[] formatting)
        {
            var formattedMessage = string.Format("[{0:uppercase=true:padding=5} {1}] {2}", 
                prefix, DateTime.Now, string.Format(message, formatting));
            //System.Diagnostics.Debug.WriteLine(formattedMessage);
            Console.WriteLine(formattedMessage);
        }

        public void Debug(string message, params object[] formatting)
        {
            WriteLog("Debug", message, formatting);
        }

        public void Debug(Func<string> message)
        {
            WriteLog("Debug", message());
        }

        public void Info(string message, params object[] formatting)
        {
            WriteLog("Debug", message, formatting);
        }

        public void Info(Func<string> message)
        {
            WriteLog("Debug", message());
        }

        public void Warn(string message, params object[] formatting)
        {
            WriteLog("Debug", message, formatting);
        }

        public void Warn(Func<string> message)
        {
            WriteLog("Debug", message());
        }

        public void Error(string message, params object[] formatting)
        {
            WriteLog("Debug", message, formatting);
        }

        public void Error(Func<string> message)
        {
            WriteLog("Debug", message());
        }

        public void Fatal(string message, params object[] formatting)
        {
            WriteLog("Debug", message, formatting);
        }

        public void Fatal(Func<string> message)
        {
            WriteLog("Debug", message());
        }

        public void Exception(string message, Exception exception)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder
                .AppendLine(message)
                .AppendFormat("Message: {0}", exception.Message)
                .AppendLine()
                .AppendLine("Stack Trace:")
                .Append(exception.StackTrace);
            WriteLog("Error", messageBuilder.ToString());
        }
    }
}