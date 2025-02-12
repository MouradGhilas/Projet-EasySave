using System;
using Livrable1.Models;
using Livrable1.Enums;
using Livrable1.Services;
using System.Text.Json;

class Program
{
    static async Task Main()
    {
        // Crée le dossier State s'il n'existe pas
        if (!Directory.Exists("State"))
        {
            Directory.CreateDirectory("State");
        }

        Logger logger = new Logger("Logs");
        BackupManager manager = new BackupManager(logger);

        string title = "=== EasySave Console App ===";
        string option1 = "1. Créer un travail de sauvegarde";
        string option2 = "2. Lister les travaux de sauvegarde";
        string option3 = "3. Exécuter une sauvegarde";
        string option4 = "4. Exécuter toutes les sauvegardes";
        string option5 = "5. Afficher les logs";
        string quitterApp = "6. Quitter";
        string choixTexte = "Choisissez une option :";
        string optionInvalide = "Option invalide. Veuillez réessayer.";

        while (true)
        {
            Console.WriteLine("Choisissez votre langue / Choose your language (fr/en) :");
            string lang = Console.ReadLine().ToLower();

            if (lang == "fr")
            {
                Console.WriteLine(title);
                Console.WriteLine(option1);
                Console.WriteLine(option2);
                Console.WriteLine(option3);
                Console.WriteLine(option4);
                Console.WriteLine(option5);
                Console.WriteLine(quitterApp);
                Console.WriteLine(choixTexte);
            }
            else if (lang == "en")
            {
                optionInvalide = await TraduireTexte(optionInvalide, "fr", "en");
 
                Console.WriteLine(await TraduireTexte(title, "fr", "en"));
                Console.WriteLine(await TraduireTexte(option1, "fr", "en"));
                Console.WriteLine(await TraduireTexte(option2, "fr", "en"));
                Console.WriteLine(await TraduireTexte(option3, "fr", "en"));
                Console.WriteLine(await TraduireTexte(option4, "fr", "en"));
                Console.WriteLine(await TraduireTexte(option5, "fr", "en"));
                Console.WriteLine(await TraduireTexte(quitterApp, "fr", "en"));
                Console.WriteLine(await TraduireTexte(choixTexte, "fr", "en"));
            }
 
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
                    Console.WriteLine(optionInvalide);
                    break;
            }
        }
    }

    static async Task<string> TraduireTexte(string texte, string sourceLang, string targetLang)
        {
            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeDataString(texte)}";
 
            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);
 
            // Décoder la réponse JSON
            using JsonDocument json = JsonDocument.Parse(response);
            string translatedText = json.RootElement[0][0][0].GetString();
 
            return translatedText;
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