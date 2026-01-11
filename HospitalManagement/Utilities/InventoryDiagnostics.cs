using System;
using System.IO;
using System.Text;

namespace HospitalManagement.Utilities
{
    public static class InventoryDiagnostics
    {
        private static readonly object _lock = new object();
        private static readonly string _logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string _logFile = Path.Combine(_logDir, "inventory.log");

        public static void Log(Exception ex, string context = null)
        {
            try
            {
                EnsureLogDir();
                var sb = new StringBuilder();
                sb.AppendLine("========================================");
                sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                if (!string.IsNullOrEmpty(context)) sb.AppendLine("Context: " + context);
                sb.AppendLine("Exception: " + ex.GetType().FullName);
                sb.AppendLine("Message: " + ex.Message);
                sb.AppendLine("StackTrace:");
                sb.AppendLine(ex.StackTrace ?? string.Empty);
                if (ex.InnerException != null)
                {
                    sb.AppendLine("InnerException: " + ex.InnerException.Message);
                    sb.AppendLine(ex.InnerException.StackTrace ?? string.Empty);
                }
                sb.AppendLine();

                lock (_lock)
                {
                    File.AppendAllText(_logFile, sb.ToString());
                }
            }
            catch
            {
                // avoid throwing from logger
            }
        }

        public static void LogMessage(string message)
        {
            try
            {
                EnsureLogDir();
                var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}";
                lock (_lock)
                {
                    File.AppendAllText(_logFile, line);
                }
            }
            catch { }
        }

        private static void EnsureLogDir()
        {
            if (!Directory.Exists(_logDir))
                Directory.CreateDirectory(_logDir);
        }
    }
}

