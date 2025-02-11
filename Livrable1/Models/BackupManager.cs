using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Livrable1.Models
{
    public class BackupManager
    {
        // Liste privée contenant les jobs de sauvegarde
        private List<BackupJob> backupJobs;

        // Constructeur qui initialise la liste
        public BackupManager()
        {
            backupJobs = new List<BackupJob>();
        }

        // Ajoute un nouveau job de sauvegarde à la liste
        public void AddJob(BackupJob job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));

            backupJobs.Add(job);
            Console.WriteLine($"Job '{job.Name}' ajouté.");
        }

        // Exécute le job correspondant à l'index fourni (jobNumber)
        public void ExecuteJob(int jobNumber)
        {
            if (jobNumber < 0 || jobNumber >= backupJobs.Count)
            {
                Console.WriteLine("Numéro de job invalide.");
                return;
            }

            BackupJob job = backupJobs[jobNumber];
            try
            {
                job.Execute();
                Console.WriteLine($"Job '{job.Name}' exécuté avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution du job '{job.Name}': {ex.Message}");
            }
        }

        // Exécute tous les jobs de sauvegarde de manière séquentielle
        public void ExecuteSequentialJobs()
        {
            Console.WriteLine("Exécution séquentielle de tous les jobs de sauvegarde...");
            foreach (BackupJob job in backupJobs)
            {
                try
                {
                    job.Execute();
                    Console.WriteLine($"Job '{job.Name}' exécuté avec succès.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'exécution du job '{job.Name}': {ex.Message}");
                }
            }
        }

        // Charge la liste des jobs à partir d'un fichier JSON (jobs.json)
        public void LoadJobs()
        {
            string filePath = "jobs.json";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Aucun fichier de jobs trouvé pour le chargement.");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                // Désérialisation dans la liste. Si le résultat est null, on initialise une nouvelle liste.
                backupJobs = JsonConvert.DeserializeObject<List<BackupJob>>(json) ?? new List<BackupJob>();
                Console.WriteLine("Jobs chargés avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des jobs: {ex.Message}");
            }
        }

        // Sauvegarde la liste des jobs dans un fichier JSON (jobs.json)
        public void SaveJobs()
        {
            string filePath = "jobs.json";
            try
            {
                string json = JsonConvert.SerializeObject(backupJobs, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Console.WriteLine("Jobs sauvegardés avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde des jobs: {ex.Message}");
            }
        }
    }
}
