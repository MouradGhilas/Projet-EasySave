using System;
using Livrable1.Models;
using Livrable1.Enums;
using Livrable1.Services;

class Program
{
    static void Main()
    {
        // Crée le dossier State s'il n'existe pas
        if (!Directory.Exists("State"))
        {
            Directory.CreateDirectory("State");
        }

        Logger logger = new Logger("Logs");
        BackupManager manager = new BackupManager(logger);

        while (true)
        {
            Console.WriteLine("=== Menu Principal ===");
            Console.WriteLine("1. Créer un travail de sauvegarde");
            Console.WriteLine("2. Lister les travaux de sauvegarde");
            Console.WriteLine("3. Exécuter une sauvegarde");
            Console.WriteLine("4. Exécuter toutes les sauvegardes");
            Console.WriteLine("5. Afficher les logs");
            Console.WriteLine("6. Quitter");
            Console.Write("Choisissez une option : ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateBackupJob(manager, logger);
                    break;
                case "2":
                    manager.ListJobs();
                    break;
                case "3":
                    ExecuteSelectedBackup(manager);
                    break;
                case "4":
                    manager.ExecuteAll();
                    break;
                case "5":
                    ShowLogs();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Option invalide. Veuillez réessayer.");
                    break;
            }
        }
    }

    static void CreateBackupJob(BackupManager manager, Logger logger)
    {
        Console.Write("Nom de la sauvegarde : ");
        string name = Console.ReadLine();

        Console.Write("Répertoire source : ");
        string sourceDirectory = Console.ReadLine();

        Console.Write("Répertoire cible : ");
        string targetDirectory = Console.ReadLine();

        Console.Write("Type de sauvegarde (1. Complète, 2. Différentielle) : ");
        string typeInput = Console.ReadLine();
        BackupType type = typeInput == "1" ? BackupType.Complete : BackupType.Differential;

        BackupJob job = new BackupJob(name, sourceDirectory, targetDirectory, type, logger);
        manager.AddJob(job);
    }

    static void ExecuteSelectedBackup(BackupManager manager)
    {
        Console.Write("Entrez les numéros des sauvegardes à exécuter (ex: 1-3 ou 1;3) : ");
        string input = Console.ReadLine();
        manager.ExecuteSelected(input);
    }

    static void ShowLogs()
    {
        string logFilePath = Path.Combine("Logs", $"{DateTime.Now:yyyy-MM-dd}.json");
        if (File.Exists(logFilePath))
        {
            string logs = File.ReadAllText(logFilePath);
            Console.WriteLine("=== Logs ===");
            Console.WriteLine(logs);
        }
        else
        {
            Console.WriteLine("Aucun log disponible pour aujourd'hui.");
        }
    }
}