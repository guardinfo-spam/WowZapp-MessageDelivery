using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace LOLMessageDelivery.Classes
{

    
    public class CommonLogger
    {
        static object _lockObject = new object();

        #region Methods
        /// <summary>
        /// Launches the debugger.
        /// </summary>
        public static void LaunchDebugger()
        {
#if DEBUGGER
#warning DEBUGGER derective is defined.Please comment the derective to run 
            Debugger.Launch();
#endif
        }

        /// <summary>
        /// Finds the line and column.
        /// </summary>
        /// <param name="ex">The target Exception.</param>
        /// <returns>result as string</returns>
        private static string FindLineAndColumn(Exception ex)
        {
            string result = string.Empty;
            var trace = new StackTrace(ex, true);

            StackFrame sf = trace.GetFrame(0);
            if (sf != null)
            {
                result = string.Format("Method Name:{0} \n Line: {1} \n Column: {2} ", sf.GetMethod().Name,
                                       sf.GetFileLineNumber(), sf.GetFileColumnNumber());
            }

            return result;
        }

        /// <summary>
        /// Converts Exception to string.
        /// </summary>
        /// <param name="ex">The target Exception.</param>
        /// <returns>result as string.</returns>
        private static string ConvertToString(Exception ex)
        {
            var sb = new StringBuilder();
            sb.Append("ErrorMessage :");
            sb.Append(ex.Message);
            sb.Append("\n");
            sb.Append("StackTrace :");
            sb.Append(ex.StackTrace);
            sb.Append("\n");
            sb.Append("Source :");
            sb.Append(ex.Source);
            sb.Append(FindLineAndColumn(ex));

            if (ex.InnerException != null)
            {
                ConvertToString(ex.InnerException);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void LogException(Exception ex)
        {
            LogException(ex, SourceCategory.General);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message)
        {
            LogError(message, SourceCategory.General);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The Exception to log.</param>
        /// <param name="category"></param>
        /// <param name="mailNotification"></param>
        public static void LogException(Exception ex, SourceCategory category, bool mailNotification)
        {
            // adds the exception correctly formatted Logging
            var logEntry = new LogEntry();
            logEntry.Message = ConvertToString(ex);
            logEntry.Categories.Add(category.ToString());
            logEntry.Priority = (int)ErrorPriority.Highest;
            logEntry.Severity = TraceEventType.Error;
            lock (_lockObject)
            {
                //Logger.Write(logEntry);
            }

        }


        public static void LogException(Exception ex, SourceCategory category)
        {
            LogException(ex, category, true);
        }



        public static void LogError(string message, SourceCategory category)
        {
            // adds the exception correctly formatted Logging
            var logEntry = new LogEntry();
            logEntry.Message = message;
            logEntry.Categories.Add(category.ToString());
            logEntry.Priority = (int)ErrorPriority.Highest;
            logEntry.Severity = TraceEventType.Error;
            logEntry.ExtendedProperties = new Dictionary<string, object>
                                              {
                                                  {"ErrorMessage", logEntry.Message}
                                              };
            lock (_lockObject)
            {
                Logger.Write(logEntry);   
            }
             
        }



        /// <summary>
        /// Logs the warn.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void LogWarn(string msg)
        {
            LogWarn(msg, SourceCategory.General);
        }


        /// <summary>
        /// Logs the warn.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category"></param>
        public static void LogWarn(string message, SourceCategory category)
        {
            LogEntry logEntry;
            if (!string.IsNullOrEmpty(message))
            {
                logEntry = new LogEntry();
                logEntry.Message = message;
                logEntry.Categories.Add(category.ToString());
                logEntry.Priority = (int)ErrorPriority.Normal;
                logEntry.Severity = TraceEventType.Warning;
                //logEntry.ExtendedProperties = new Dictionary<string, object>
                //                          {
                //                              {"ErrorMessage", message}
                //                          };
                lock (_lockObject)
                {
                    Logger.Write(logEntry);
                }

            }
        }

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogInfo(string message)
        {
            LogInfo(message, SourceCategory.General);
        }

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category"></param>
        public static void LogInfo(string message, SourceCategory category)
        {
            LogEntry logEntry;
            if (!string.IsNullOrEmpty(message))
            {
                logEntry = new LogEntry();
                logEntry.Message = message;
                logEntry.Categories.Add(category.ToString());
                logEntry.Priority = (int)ErrorPriority.Normal;
                logEntry.Severity = TraceEventType.Information;
                //logEntry.ExtendedProperties = new Dictionary<string, object>
                //                          {
                //                              {"ErrorMessage", message}
                //                          };

                lock (_lockObject)
                {
                    Logger.Write(logEntry);
                }
            }
        }

        public static void LogInfoFormat(string message, SourceCategory category, params object[] objs)
        {
            LogInfo(string.Format(message, objs), category);
        }

        #endregion

    }

    /// <summary>
    /// Enumeration of predefined categories of errors.
    /// </summary>
    public enum ErrorCategories : byte
    {
        /// <summary>
        /// general error category.
        /// </summary>
        General = 1,

        /// <summary>
        /// error category.
        /// </summary>
        Error = 2,

        /// <summary>
        /// warning category.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// trace category.
        /// </summary>
        Trace = 4,

    }

    public enum ErrorPriority : byte
    {
        /// <summary>
        /// Lowest priority.
        /// </summary>
        Lowest = 0,

        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 1,

        /// <summary>
        /// Normal priority.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Priority is above normal (and also below High).
        /// </summary>
        AboveNormal = 3,

        /// <summary>
        /// High priority.
        /// </summary>
        High = 4,

        /// <summary>
        /// Highest priority.
        /// </summary>
        Highest = 5
    }

    public enum SourceCategory
    {
        General
    }
}