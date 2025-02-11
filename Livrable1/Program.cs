using System;
using System.Collections.Generic;

namespace BackupSystem
{
    // Enumération pour le type de sauvegarde.
    public enum BackupType
    {
        Complete,
        Differential
    }

    // 1. Classe BackupJob
    // Représente un travail de sauvegarde unique.
    public class BackupJob
    {
        // Attributs
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public BackupType Type { get; set; }

        // Constructeur
        public BackupJob(string name, string sourceDirectory, string targetDirectory, BackupType type)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            Type = type;
        }

        // Méthode de validation des paramètres.
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(SourceDirectory) ||
                string.IsNullOrWhiteSpace(TargetDirectory))
            {
                Console.WriteLine($"[BackupJob: {Name}] Validation échouée : les champs ne doivent pas être vides.");
                return false;
            }
            Console.WriteLine($"[BackupJob: {Name}] Validation réussie.");
            return true;
        }

        // Méthode d'exécution du travail de sauvegarde.
        public void Execute()
        {
            if (!Validate())
            {
                Console.WriteLine($"[BackupJob: {Name}] Échec de l'exécution en raison d'une validation non conforme.");
                return;
            }
            // Ici, on simule le processus de sauvegarde.
            Console.WriteLine($"[BackupJob: {Name}] Exécution de la sauvegarde ({Type}) de '{SourceDirectory}' vers '{TargetDirectory}'.");
        }
    }

    // 2. Classe Logger
    // Responsable d'enregistrer les actions dans un fichier de log (ici simulé par la console).
    public class Logger
    {
        public string LogFilePath { get; set; }

        public Logger(string logFilePath)
        {
            LogFilePath = logFilePath;
        }

        // Ajoute une entrée au log.
        public void WriteLog(string entry)
        {
            // Pour une application réelle, on pourrait écrire dans un fichier via File.AppendAllText.
            Console.WriteLine($"[Log] {DateTime.Now}: {entry}");
        }
    }

    // 3. Classe StateTracker
    // Enregistre et met à jour l’état des travaux de sauvegarde.
    public class StateTracker
    {
        public string StateFilePath { get; set; }

        public StateTracker(string stateFilePath)
        {
            StateFilePath = stateFilePath;
        }

        // Met à jour l’état d’un job spécifique.
        public void UpdateState(BackupJob job)
        {
            // Pour une application réelle, on pourrait sérialiser l'état dans un fichier JSON.
            Console.WriteLine($"[StateTracker] Mise à jour de l'état du job '{job.Name}' dans '{StateFilePath}'.");
        }
    }

    // 4. Classe BackupManager
    // Gère la liste des travaux de sauvegarde et coordonne leurs exécutions.
    public class BackupManager
    {
        // Liste de BackupJob (Association : un BackupManager peut gérer 0..* BackupJob).
        private List<BackupJob> backupJobs = new List<BackupJob>();

        // Dépendances (Logger et StateTracker)
        private Logger logger;
        private StateTracker stateTracker;

        // Constructeur qui reçoit les dépendances nécessaires.
        public BackupManager(Logger logger, StateTracker stateTracker)
        {
            this.logger = logger;
            this.stateTracker = stateTracker;
        }

        // Ajoute un nouveau job de sauvegarde.
        public void AddJob(BackupJob job)
        {
            backupJobs.Add(job);
            logger.WriteLog($"Ajout du job de sauvegarde : {job.Name}");
        }

        // Supprime un job en fonction de son nom.
        public void RemoveJob(string name)
        {
            BackupJob job = GetJob(name);
            if (job != null)
            {
                backupJobs.Remove(job);
                logger.WriteLog($"Suppression du job de sauvegarde : {name}");
            }
            else
            {
                logger.WriteLog($"Tentative de suppression : le job '{name}' n'existe pas.");
            }
        }

        // Récupère un job par son nom.
        public BackupJob GetJob(string name)
        {
            return backupJobs.Find(j => j.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Exécute tous les jobs de sauvegarde de manière séquentielle.
        public void ExecuteAll()
        {
            foreach (var job in backupJobs)
            {
                logger.WriteLog($"Démarrage de l'exécution du job : {job.Name}");
                job.Execute();
                stateTracker.UpdateState(job);
                logger.WriteLog($"Fin de l'exécution du job : {job.Name}");
            }
        }
    }

    // Classe Program pour démontrer l'utilisation du système.
    class Program
    {
        static void Main(string[] args)
        {
            // Instanciation des dépendances
            Logger logger = new Logger("backup.log");
            StateTracker stateTracker = new StateTracker("state.json");

            // Création du BackupManager (relation d'association et dépendance)
            BackupManager backupManager = new BackupManager(logger, stateTracker);

            // Création de quelques BackupJob
            BackupJob job1 = new BackupJob("Job1", @"C:\Données\Source1", @"D:\Sauvegardes\Target1", BackupType.Complete);
            BackupJob job2 = new BackupJob("Job2", @"C:\Données\Source2", @"D:\Sauvegardes\Target2", BackupType.Differential);

            // Ajout des jobs dans le BackupManager
            backupManager.AddJob(job1);
            backupManager.AddJob(job2);

            // Exécution de tous les jobs
            backupManager.ExecuteAll();

            // Suppression d'un job
            backupManager.RemoveJob("Job1");

            // Vérification de la suppression
            if (backupManager.GetJob("Job1") == null)
            {
                Console.WriteLine("Le job 'Job1' a bien été supprimé.");
            }

            Console.WriteLine("Fin du programme.");
        }
    }
}
