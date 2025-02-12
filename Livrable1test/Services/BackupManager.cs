using System;
using System.Collections.Generic;
using Livrable1.Models;
using LoggerLibrary;

namespace Livrable1.Services
{
    public class BackupManager
    {
        private const int MAX_BACKUPS = 5;
        private readonly List<BackupJob> _backupJobs;
        private readonly Logger _logger;

        public BackupManager(Logger logger)
        {
            _backupJobs = new List<BackupJob>();
            _logger = logger;
        }

        public void AddJob(BackupJob job)
        {
            if (_backupJobs.Count >= MAX_BACKUPS)
            {
                _logger.LogMessage("Erreur : Limite de 5 sauvegardes atteinte.");
                return;
            }

            _backupJobs.Add(job);
            _logger.LogMessage($"Job ajouté : {job.Name}");
        }

        public void ListJobs()
        {
            if (_backupJobs.Count == 0)
            {
                Console.WriteLine("Aucun travail de sauvegarde enregistré.");
                return;
            }

            Console.WriteLine("Travaux de sauvegarde enregistrés :");
            for (int i = 0; i < _backupJobs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_backupJobs[i].Name}");
            }
        }
        
        public void ExecuteJob(int index)
        {
            if (index < 0 || index >= _backupJobs.Count)
            {
                _logger.LogMessage("Erreur : Index invalide.");
                return;
            }

            try
            {
                _backupJobs[index].Execute();
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Erreur lors de l'exécution de {_backupJobs[index].Name} : {ex.Message}");
            }
        }

        public void ExecuteAll()
        {
            if (_backupJobs.Count == 0)
            {
                _logger.LogMessage("Aucun travail de sauvegarde à exécuter.");
                return;
            }

            foreach (var job in _backupJobs)
            {
                try
                {
                    job.Execute();
                }
                catch (Exception ex)
                {
                    _logger.LogMessage($"Erreur sur {job.Name} : {ex.Message}");
                }
            }
        }

        public void ExecuteSelected(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                _logger.LogMessage("Erreur : Aucune sélection fournie.");
                return;
            }

            try
            {
                if (input.Contains("-")) // Exemple : "1-3"
                {
                    string[] range = input.Split('-');
                    int start = int.Parse(range[0]) - 1;
                    int end = int.Parse(range[1]) - 1;

                    for (int i = start; i <= end; i++)
                    {
                        ExecuteJob(i);
                    }
                }
                else if (input.Contains(";")) // Exemple : "1;3"
                {
                    string[] indices = input.Split(';');
                    foreach (string indexStr in indices)
                    {
                        int index = int.Parse(indexStr) - 1;
                        ExecuteJob(index);
                    }
                }
                else // Exemple : "1"
                {
                    int index = int.Parse(input) - 1;
                    ExecuteJob(index);
                }
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Erreur lors de l'exécution sélective : {ex.Message}");
            }
        }
    }
}