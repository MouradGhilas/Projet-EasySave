using Livrable1.Models; // Importer Logger et StateMonitor et BackupJob

namespace Livrable1
{
    class Program
    {
        static Logger logger = new Logger("/Users/maxencechartier/SourceFolder/log.txt");
        static StateMonitor stateMonitor = new StateMonitor("/Users/maxencechartier/SourceFolder/state.json");

        static void Main(string[] args)
        {
            Console.WriteLine("=== EasySave Console App ===");
            Console.WriteLine("1. Créer et valider un travail de sauvegarde");
            Console.WriteLine("2. Exécuter un travail de sauvegarde");
            Console.WriteLine("3. Tester Logger");
            Console.WriteLine("4. Tester StateMonitor");
            Console.WriteLine("Choisissez une option :");

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
                Console.WriteLine("Option invalide.");
            }
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
