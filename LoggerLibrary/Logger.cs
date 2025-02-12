using System;
using System.IO;
using System.Text.Json;

namespace LoggerLibrary
{
    public class Logger
    {
        private readonly string _logDirectory;
    
        public Logger(string logDirectory)
        {
            _logDirectory = logDirectory;
    
            // Crée le répertoire de logs s'il n'existe pas
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
    
        // Méthode pour écrire un log détaillé au format JSON
        public void LogAction(string backupName, string sourceFilePath, string targetFilePath, long fileSize, long transferTimeMs)
        {
            // Crée un objet logEntry avec les informations nécessaires
            var logEntry = new
            {
                Timestamp = DateTime.Now,
                BackupName = backupName,
                SourceFilePath = sourceFilePath,
                TargetFilePath = targetFilePath,
                FileSize = fileSize,
                TransferTimeMs = transferTimeMs
            };
    
            // Sérialise l'objet en JSON sans encoder les caractères spéciaux
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonLogEntry = JsonSerializer.Serialize(logEntry, options);
    
            // Détermine le fichier de log journalier
            string logFilePath = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}.json");
    
            // Ajoute l'entrée de log au fichier
            File.AppendAllText(logFilePath, jsonLogEntry + Environment.NewLine);
        }
    
        // Méthode pour écrire un log simple (message d'erreur ou information générale)
        public void LogMessage(string message)
        {
            // Crée un objet logEntry pour les messages simples
            var logEntry = new
            {
                Timestamp = DateTime.Now,
                Message = message
            };
    
            // Sérialise l'objet en JSON sans encoder les caractères spéciaux
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonLogEntry = JsonSerializer.Serialize(logEntry, options);
    
            // Détermine le fichier de log journalier
            string logFilePath = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}.json");
    
            // Ajoute l'entrée de log au fichier
            File.AppendAllText(logFilePath, jsonLogEntry + Environment.NewLine);
        }
    }
}