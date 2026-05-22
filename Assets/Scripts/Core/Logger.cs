using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace SesiDefense.Core
{
    /// <summary>
    /// Centralized logging system for the entire game.
    /// Supports file logging, console output, and filtering.
    /// </summary>
    public static class Logger
    {
        private static string logFilePath;
        private static bool enableFileLogging = true;
        private static LogLevel minimumLogLevel = LogLevel.Info;
        private static List<LogEntry> logHistory = new List<LogEntry>();
        private const int MAX_LOG_HISTORY = 10000;

        static Logger()
        {
            InitializeLogging();
        }

        private static void InitializeLogging()
        {
            string logsDirectory = Application.persistentDataPath + "/Logs";
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            logFilePath = Path.Combine(logsDirectory, $"sesidefense_{timestamp}.log");
        }

        /// <summary>
        /// Logs a message with specified log level.
        /// </summary>
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            if (level < minimumLogLevel)
                return;

            string timestamp = System.DateTime.Now.ToString("HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] [{level}] {message}";

            // Console output with color
            string coloredMessage = GetColoredMessage(message, level);
            Debug.Log(coloredMessage);

            // File logging
            if (enableFileLogging)
            {
                try
                {
                    File.AppendAllText(logFilePath, logMessage + "\n");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to write to log file: {ex.Message}");
                }
            }

            // History
            logHistory.Add(new LogEntry
            {
                timestamp = timestamp,
                level = level,
                message = message
            });

            if (logHistory.Count > MAX_LOG_HISTORY)
            {
                logHistory.RemoveAt(0);
            }
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        public static void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        public static void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }

        /// <summary>
        /// Logs a debug message (only in development).
        /// </summary>
        public static void LogDebug(string message)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Log(message, LogLevel.Debug);
#endif
        }

        /// <summary>
        /// Gets colored message for console output.
        /// </summary>
        private static string GetColoredMessage(string message, LogLevel level)
        {
            return level switch
            {
                LogLevel.Error => $"<color=red>[ERROR] {message}</color>",
                LogLevel.Warning => $"<color=yellow>[WARNING] {message}</color>",
                LogLevel.Debug => $"<color=cyan>[DEBUG] {message}</color>",
                _ => $"<color=white>[INFO] {message}</color>"
            };
        }

        /// <summary>
        /// Sets the minimum log level to output.
        /// </summary>
        public static void SetMinimumLogLevel(LogLevel level)
        {
            minimumLogLevel = level;
        }

        /// <summary>
        /// Gets the log history.
        /// </summary>
        public static List<LogEntry> GetLogHistory()
        {
            return new List<LogEntry>(logHistory);
        }

        /// <summary>
        /// Clears the log history.
        /// </summary>
        public static void ClearHistory()
        {
            logHistory.Clear();
        }

        /// <summary>
        /// Exports logs to file.
        /// </summary>
        public static void ExportLogs()
        {
            if (enableFileLogging)
            {
                Logger.Log($"Logs exported to: {logFilePath}", LogLevel.Info);
            }
        }
    }

    /// <summary>
    /// Log level enumeration.
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    /// <summary>
    /// Log entry record.
    /// </summary>
    public class LogEntry
    {
        public string timestamp { get; set; }
        public LogLevel level { get; set; }
        public string message { get; set; }
    }
}
