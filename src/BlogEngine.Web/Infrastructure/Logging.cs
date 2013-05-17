using System;
using System.Collections.Concurrent;
using System.Text;

namespace BlogEngine
{
    /// <summary>
    /// Extensions to help make logging awesome
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Concurrent dictionary that ensures only one instance of a logger for a type.
        /// </summary>
        private static readonly Lazy<ConcurrentDictionary<string, ILog>> Dictionary =
            new Lazy<ConcurrentDictionary<string, ILog>>(() => new ConcurrentDictionary<string, ILog>());

        /// <summary>
        /// Gets the logger for <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type to get the logger for.</param>
        /// <returns>Instance of a logger for the object.</returns>
        public static ILog Log<T>(this T type)
        {
            string objectName = typeof(T).FullName;
            return Log(objectName);
        }

        /// <summary>
        /// Gets the logger for the specified object name.
        /// </summary>
        /// <param name="objectName">Either use the fully qualified object name or the short. If used with Log&lt;T&gt;() you must use the fully qualified object name"/></param>
        /// <returns>Instance of a logger for the object.</returns>
        public static ILog Log(this string objectName)
        {
            return Dictionary.Value.GetOrAdd(objectName, BlogEngine.Log.GetLoggerFor);
        }
    }

    /// <summary>
    /// Logger type initialization
    /// </summary>
    public static class Log
    {
        private static Type _logType = typeof(NullLog);
        private static ILog _logger;

        /// <summary>
        /// Sets up logging to be with a certain type
        /// </summary>
        /// <typeparam name="T">The type of ILog for the application to use</typeparam>
        public static void InitializeWith<T>() where T : ILog, new()
        {
            _logType = typeof(T);
        }

        /// <summary>
        /// Sets up logging to be with a certain instance. The other method is preferred.
        /// </summary>
        /// <param name="loggerType">Type of the logger.</param>
        /// <remarks>This is mostly geared towards testing</remarks>
        public static void InitializeWith(ILog loggerType)
        {
            _logType = loggerType.GetType();
            _logger = loggerType;
        }

        /// <summary>
        /// Initializes a new instance of a logger for an object.
        /// This should be done only once per object name.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <returns>ILog instance for an object if log type has been intialized; otherwise null</returns>
        public static ILog GetLoggerFor(string objectName)
        {
            var logger = _logger;

            if (_logger == null)
            {
                logger = Activator.CreateInstance(_logType) as ILog;
                if (logger != null)
                {
                    logger.InitializeFor(objectName);
                }
            }

            return logger;
        }
    }

    /// <summary>
    /// Custom interface for logging messages
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Initializes the instance for the logger name
        /// </summary>
        /// <param name="loggerName">Name of the logger</param>
        void InitializeFor(string loggerName);

        /// <summary>
        /// Debug level of the specified message. The other method is preferred since the execution is deferred.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatting">The formatting.</param>
        void Debug(string message, params object[] formatting);

        /// <summary>
        /// Debug level of the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(Func<string> message);

        /// <summary>
        /// Info level of the specified message. The other method is preferred since the execution is deferred.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatting">The formatting.</param>
        void Info(string message, params object[] formatting);

        /// <summary>
        /// Info level of the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(Func<string> message);

        /// <summary>
        /// Warn level of the specified message. The other method is preferred since the execution is deferred.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatting">The formatting.</param>
        void Warn(string message, params object[] formatting);

        /// <summary>
        /// Warn level of the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(Func<string> message);

        /// <summary>
        /// Error level of the specified message. The other method is preferred since the execution is deferred.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatting">The formatting.</param>
        void Error(string message, params object[] formatting);

        /// <summary>
        /// Error level of the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(Func<string> message);

        /// <summary>
        /// Fatal level of the specified message. The other method is preferred since the execution is deferred.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatting">The formatting.</param>
        void Fatal(string message, params object[] formatting);

        /// <summary>
        /// Fatal level of the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(Func<string> message);

        /// <summary>
        /// Error level of the specified message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception</param>
        void Exception(string message, Exception exception);
    }

    /// <summary>
    /// Ensures a default constructor for the logger type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILog<T> where T : new()
    {
    }

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
