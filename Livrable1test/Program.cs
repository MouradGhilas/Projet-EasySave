using System;
using System.IO;
using System.Threading.Tasks;
using Livrable1.Models;
using Livrable1.Enums;
using Livrable1.Services;
using LoggerLibrary;

class Program
{
    static async Task Main()
    {
        // Crée le dossier State s'il n'existe pas
        if (!Directory.Exists("State"))
        {
            Directory.CreateDirectory("State");
        }

        // Demander à l'utilisateur de choisir le format des logs
        Console.WriteLine("Choisissez le format des logs (1. JSON, 2. XML) :");
        string logFormatChoice = Console.ReadLine() ?? "1";
        LogFormat logFormat = logFormatChoice == "1" ? LogFormat.Json : LogFormat.Xml;

        Logger logger = new Logger("Logs", logFormat);
        BackupManager manager = new BackupManager(logger);

        string lang;
        while (true)
        {
            Console.WriteLine("Choisissez votre langue / Choose your language (fr/en) :");
            lang = Console.ReadLine()?.ToLower() ?? "fr";

            if (lang == "fr" || lang == "en")
                break;

            Console.WriteLine("Langue invalide. Veuillez entrer 'fr' ou 'en'.");
        }

        while (true)
        {
            string title = "=== EasySave Console App ===";
            string option1 = "1. Créer un travail de sauvegarde";
            string option2 = "2. Lister les travaux de sauvegarde";
            string option3 = "3. Exécuter une sauvegarde";
            string option4 = "4. Exécuter toutes les sauvegardes";
            string option5 = "5. Afficher les logs";
            string quitterApp = "6. Quitter";
            string choixTexte = "Choisissez une option :";
            string optionInvalide = "Option invalide. Veuillez réessayer.";

            if (lang == "en")
            {
                title = await Translator.TraduireTexte(title, "fr", "en");
                option1 = await Translator.TraduireTexte(option1, "fr", "en");
                option2 = await Translator.TraduireTexte(option2, "fr", "en");
                option3 = await Translator.TraduireTexte(option3, "fr", "en");
                option4 = await Translator.TraduireTexte(option4, "fr", "en");
                option5 = await Translator.TraduireTexte(option5, "fr", "en");
                quitterApp = await Translator.TraduireTexte(quitterApp, "fr", "en");
                choixTexte = await Translator.TraduireTexte(choixTexte, "fr", "en");
                optionInvalide = await Translator.TraduireTexte(optionInvalide, "fr", "en");
            }

            Console.WriteLine(title);
            Console.WriteLine(option1);
            Console.WriteLine(option2);
            Console.WriteLine(option3);
            Console.WriteLine(option4);
            Console.WriteLine(option5);
            Console.WriteLine(quitterApp);
            Console.WriteLine(choixTexte);

            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    await CreateBackupJob(manager, logger, lang);
                    break;
                case "2":
                    manager.ListJobs();
                    break;
                case "3":
                    await ExecuteSelectedBackup(manager, lang);
                    break;
                case "4":
                    manager.ExecuteAll();
                    break;
                case "5":
                    await ShowLogs(lang, logFormat);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine(optionInvalide);
                    break;
            }
        }
    }

    static async Task CreateBackupJob(BackupManager manager, Logger logger, string lang)
    {
        string name, sourceDirectory, targetDirectory, typeInput;
        BackupType type;

        if (lang == "en")
        {
            Console.Write(await Translator.TraduireTexte("Nom de la sauvegarde : ", "fr", "en"));
            name = Console.ReadLine() ?? string.Empty;

            Console.Write(await Translator.TraduireTexte("Répertoire source : ", "fr", "en"));
            sourceDirectory = Console.ReadLine() ?? string.Empty;

            Console.Write(await Translator.TraduireTexte("Répertoire cible : ", "fr", "en"));
            targetDirectory = Console.ReadLine() ?? string.Empty;

            Console.Write(await Translator.TraduireTexte("Type de sauvegarde (1. Complète, 2. Différentielle) : ", "fr", "en"));
            typeInput = Console.ReadLine() ?? "1";
        }
        else
        {
            Console.Write("Nom de la sauvegarde : ");
            name = Console.ReadLine() ?? string.Empty;

            Console.Write("Répertoire source : ");
            sourceDirectory = Console.ReadLine() ?? string.Empty;

            Console.Write("Répertoire cible : ");
            targetDirectory = Console.ReadLine() ?? string.Empty;

            Console.Write("Type de sauvegarde (1. Complète, 2. Différentielle) : ");
            typeInput = Console.ReadLine() ?? "1";
        }

        type = typeInput == "1" ? BackupType.Complete : BackupType.Differential;

        BackupJob job = new BackupJob(name, sourceDirectory, targetDirectory, type, logger);
        manager.AddJob(job);
    }

    static async Task ExecuteSelectedBackup(BackupManager manager, string lang)
    {
        if (lang == "en")
        {
            Console.Write(await Translator.TraduireTexte("Entrez les numéros des sauvegardes à exécuter (ex: 1-3 ou 1;3) : ", "fr", "en"));
        }
        else
        {
            Console.Write("Entrez les numéros des sauvegardes à exécuter (ex: 1-3 ou 1;3) : ");
        }

        string input = Console.ReadLine() ?? string.Empty;
        manager.ExecuteSelected(input);
    }

    static async Task ShowLogs(string lang, LogFormat logFormat)
    {
        string logFilePath = Path.Combine("Logs", $"{DateTime.Now:yyyy-MM-dd}.{logFormat.ToString().ToLower()}");
        if (File.Exists(logFilePath))
        {
            string logs = File.ReadAllText(logFilePath);
            Console.WriteLine("=== Logs ===");
            Console.WriteLine(logs);
        }
        else
        {
            string noLogsMessage = "Aucun log disponible pour aujourd'hui.";

            if (lang == "en")
            {
                noLogsMessage = await Translator.TraduireTexte(noLogsMessage, "fr", "en");
            }

            Console.WriteLine(noLogsMessage);
        }
    }
}