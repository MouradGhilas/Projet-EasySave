using System;
using System.IO;

namespace Livrable1.Models
{
    public class Logger
    {
        public string LogFilePath { get; private set; }

        public Logger(string logFilePath)
        {
            LogFilePath = logFilePath;

            // Vérifier si le fichier de log existe déjà, sinon le créer
            if (!File.Exists(LogFilePath))
            {
                SetupLogFile();
            }
        }

        public void SetupLogFile()
        {
            try
            {
                using (var stream = File.Create(LogFilePath))
                {
                    Console.WriteLine($"Fichier de log créé : {LogFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du fichier de log : {ex.Message}");
            }
        }

        public void LogAction(string actionDetails)
        {
            try
            {
                string logEntry = $"{DateTime.Now}: {actionDetails}";
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                Console.WriteLine($"Action loggée : {logEntry}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture dans le log : {ex.Message}");
            }
        }
    }
}
