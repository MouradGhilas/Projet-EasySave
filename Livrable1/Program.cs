using Livrable1.Models; // Importer Logger et StateMonitor
using System.Text.Json;


namespace Livrable1
{
    class Program
    {
        //create the file log.txt
        static Logger logger = new Logger("Livrable1/log");

        //specifications about the state of the state of the backup
        static StateMonitor stateMonitor = new StateMonitor("/Users/maxencechartier/SourceFolder/state.json");


        //console interface : MENU
        static async Task Main(string[] args)
        static Logger logger = new Logger("/Users/maxencechartier/SourceFolder/log.txt");
        static StateMonitor stateMonitor = new StateMonitor("/Users/maxencechartier/SourceFolder/state.json");

        static void Main(string[] args)
        {

            Console.WriteLine("Choisissez votre langue / Choose your language (fr/en) :");
            string lang = Console.ReadLine().ToLower();

            string title = "=== EasySave Console App ===";
            string option1 = "1. Créer et valider un travail de sauvegarde";
            string option2 = "2. Exécuter un travail de sauvegarde";
            string option3 = "3. Tester Logger";
            string option4 = "4. Tester StateMonitor";
            string choixTexte = "Choisissez une option :";
            string optionInvalide = "Option invalide.";

            if (lang == "fr")
            {
                Console.WriteLine(title);
                Console.WriteLine(option1);
                Console.WriteLine(option2);
                Console.WriteLine(option3);
                Console.WriteLine(option4);
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
                Console.WriteLine(await TraduireTexte(choixTexte, "fr", "en"));
            }

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                TestValidateBackupJob();
            }
            else if (choice == "2")
            {
                TestExecuteBackupJob();
            }
            else if (choice == "3")
            {
                TestLogger();
            }
            else if (choice == "4")
            {
                TestStateMonitor();
            }
            else
            {
                Console.WriteLine(optionInvalide);
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

        static void TestLogger()
        {
            Console.WriteLine("=== Test : Logger ===");

            // Log une action de test
            logger.LogAction("Travail de sauvegarde exécuté avec succès.");
            logger.LogAction("Une autre action importante a été réalisée.");
        }

        static void TestStateMonitor()
        {
            Console.WriteLine("=== Test : StateMonitor ===");

            var job = new BackupJob(
                "TestJob",
                "/Users/maxencechartier/SourceFolder",
                "/Users/maxencechartier/TargetFolder",
                BackupType.Complete
            );

            // Simuler un état avec 5 fichiers restants
            stateMonitor.UpdateState(job, "En cours", 5, 10485760);

            // Simuler la fin du travail
            stateMonitor.UpdateState(job, "Terminé", 0, 0);
        }

        static void TestValidateBackupJob()
        {
            Console.WriteLine("=== Test : Valider un travail de sauvegarde ===");

            var job = new BackupJob(
                "TestJob",
                "/Users/maxencechartier/SourceFolder",
                "/Users/maxencechartier/TargetFolder",
                BackupType.Complete
            );

            bool isValid = job.ValidatePaths();
            Console.WriteLine(isValid
                ? "Les chemins source et cible sont valides."
                : "Les chemins source et/ou cible sont invalides.");
        }

        static void TestExecuteBackupJob()
        {
            Console.WriteLine("=== Test : Exécuter un travail de sauvegarde ===");

            var job = new BackupJob(
                "TestJob",
                "/Users/maxencechartier/SourceFolder",
                "/Users/maxencechartier/TargetFolder",
                BackupType.Complete
            );

            try
            {
                job.Execute();
                logger.LogAction($"Travail de sauvegarde {job.Name} exécuté avec succès.");
                stateMonitor.UpdateState(job, "Terminé", 0, 0);
            }
            catch (Exception ex)
            {
                logger.LogAction($"Erreur lors de l'exécution du travail {job.Name}: {ex.Message}");
                Console.WriteLine($"Erreur lors de l'exécution : {ex.Message}");
            }
        }
    }
}