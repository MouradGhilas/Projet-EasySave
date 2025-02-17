using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;

namespace LoggerLibrary
{
    public class Logger
    {
        private readonly string _logDirectory;
        private readonly LogFormat _logFormat;

        public Logger(string logDirectory, LogFormat logFormat)
        {
            _logDirectory = logDirectory;
            _logFormat = logFormat;

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public void LogMessage(string message)
        {
            string logEntry = $"{DateTime.Now}: {message}";
            string logFilePath = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}.{_logFormat.ToString().ToLower()}");
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            Console.WriteLine(logEntry);
        }

        public void LogAction(string backupName, string sourceFilePath, string targetFilePath, long fileSize, long transferTimeMs, long encryptionTimeMs = 0)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now,
                BackupName = backupName,
                SourceFilePath = sourceFilePath,
                TargetFilePath = targetFilePath,
                FileSize = fileSize,
                TransferTimeMs = transferTimeMs,
                EncryptionTimeMs = encryptionTimeMs
            };

            string logEntryString;
            if (_logFormat == LogFormat.Json)
            {
                logEntryString = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
            }
            else
            {
                logEntryString = new XElement("LogEntry",
                    new XElement("Timestamp", logEntry.Timestamp),
                    new XElement("BackupName", logEntry.BackupName),
                    new XElement("SourceFilePath", logEntry.SourceFilePath),
                    new XElement("TargetFilePath", logEntry.TargetFilePath),
                    new XElement("FileSize", logEntry.FileSize),
                    new XElement("TransferTimeMs", logEntry.TransferTimeMs),
                    new XElement("EncryptionTimeMs", logEntry.EncryptionTimeMs)
                ).ToString();
            }

            string logFilePath = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}.{_logFormat.ToString().ToLower()}");
            File.AppendAllText(logFilePath, logEntryString + Environment.NewLine);
        }
    }

    public enum LogFormat
    {
        Json,
        Xml
    }
}