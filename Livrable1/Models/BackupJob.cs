using System;
using System.IO;

namespace Livrable1.Models
{
    public enum BackupType
    {
        Complete,
        Differential
    }

    public class BackupJob
    {
        public string Name { get; private set; }
        public string SourceDirectory { get; private set; }
        public string TargetDirectory { get; private set; }
        public BackupType Type { get; private set; }

        public BackupJob(string name, string sourceDirectory, string targetDirectory, BackupType type)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            Type = type;
        }

        // management of the errors
        public bool ValidatePaths()
        {
            bool sourceExists = Directory.Exists(SourceDirectory);
            bool targetExists = Directory.Exists(TargetDirectory);

            if (!sourceExists)
                Console.WriteLine($"Erreur : Le répertoire source n'existe pas -> {SourceDirectory}");
            if (!targetExists)
                Console.WriteLine($"Erreur : Le répertoire cible n'existe pas -> {TargetDirectory}");

            return sourceExists && targetExists;
        }


        public void Execute()
        {
            if (!ValidatePaths())
            {
                throw new InvalidOperationException("Les répertoires source ou cible ne sont pas valides.");
            }

            Console.WriteLine($"Exécution du travail de sauvegarde : {Name}");
            Console.WriteLine($"Sauvegarde {Type} de {SourceDirectory} vers {TargetDirectory}.");
            // Simulez une copie ici ou ajoutez la logique réelle pour gérer la sauvegarde.
            // simulate a copy here or add the real logic to manage the backup
        }
    }
}
