using System;
using System.IO;
using System.Text.Json;
using Livrable1.Enums;
using Livrable1.Services;
using LoggerLibrary;

namespace Livrable1.Models
{
    public class BackupJob
    {
        public string Name { get; }
        public string SourceDirectory { get; }
        public string TargetDirectory { get; }
        public BackupType Type { get; }
        private DateTime _lastBackupTime;
        private readonly Logger _logger;
        private BackupState _state;

        public BackupJob(string name, string sourceDirectory, string targetDirectory, BackupType type, Logger logger)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            Type = type;
            _logger = logger;
            _lastBackupTime = DateTime.MinValue;
            _state = new BackupState
            {
                BackupName = name,
                Status = "Non Actif"
            };
        }
        public void UpdateState(string status, int totalFiles = 0, long totalSize = 0, int remainingFiles = 0, long remainingSize = 0, string currentSourceFile = null, string currentTargetFile = null)
        {
            _state.LastActionTimestamp = DateTime.Now;
            _state.Status = status;
            _state.TotalFiles = totalFiles;
            _state.TotalSize = totalSize;
            _state.RemainingFiles = remainingFiles;
            _state.RemainingSize = remainingSize;
            _state.CurrentSourceFile = currentSourceFile;
            _state.CurrentTargetFile = currentTargetFile;

            // Écrire l'état dans un fichier JSON
            WriteStateToFile();
        }
        private void WriteStateToFile()
        {
            string stateFilePath = Path.Combine("State", $"{Name}_state.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonState = JsonSerializer.Serialize(_state, options);
            File.WriteAllText(stateFilePath, jsonState);
        }

        public bool ValidatePaths()
        {
            bool sourceExists = Directory.Exists(SourceDirectory);
            bool targetExists = Directory.Exists(TargetDirectory);

            if (!sourceExists) _logger.LogMessage($"Erreur : Répertoire source inexistant -> {SourceDirectory}");
            if (!targetExists) _logger.LogMessage($"Erreur : Répertoire cible inexistant -> {TargetDirectory}");

            return sourceExists && targetExists;
        }
                public void Execute()
        {
            if (!ValidatePaths())
            {
                throw new InvalidOperationException("Répertoires invalides");
            }
    
            _logger.LogMessage($"Début de la sauvegarde : {Name}");
            UpdateState("Actif");
    
            // Vérifie que le répertoire source contient des fichiers
            string[] files = Directory.GetFiles(SourceDirectory, "*.*", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                _logger.LogMessage($"Aucun fichier trouvé dans le répertoire source : {SourceDirectory}");
                UpdateState("Terminé");
                return;
            }
    
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;
    
            UpdateState("Actif", totalFiles, totalSize, remainingFiles, remainingSize);
    
            foreach (string filePath in files)
            {
                DateTime lastModified = File.GetLastWriteTime(filePath);
    
                // Si sauvegarde différentielle et fichier non modifié depuis la dernière sauvegarde, on passe
                if (Type == BackupType.Differential && lastModified <= _lastBackupTime)
                {
                    remainingFiles--;
                    remainingSize -= new FileInfo(filePath).Length;
                    continue;
                }
    
                string relativePath = filePath.Substring(SourceDirectory.Length + 1);
                string destFile = Path.Combine(TargetDirectory, relativePath);
    
                // Crée le répertoire cible si nécessaire
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
    
                // Mesure du temps de transfert
                var startTime = DateTime.Now;
                File.Copy(filePath, destFile, true);
                var transferTimeMs = (DateTime.Now - startTime).TotalMilliseconds;
    
                // Met à jour l'état
                remainingFiles--;
                remainingSize -= new FileInfo(filePath).Length;
                UpdateState("Actif", totalFiles, totalSize, remainingFiles, remainingSize, filePath, destFile);
    
                // Log détaillé
                _logger.LogAction(Name, filePath, destFile, new FileInfo(filePath).Length, (long)transferTimeMs);
            }
    
            UpdateState("Terminé");
            _logger.LogMessage($"Sauvegarde terminée pour {Name}");
        }
    }
}