using System;
using System.IO;
using Newtonsoft.Json;

namespace Livrable1.Models
{
    public class StateMonitor
    {
        public string StateFilePath { get; private set; }

        public StateMonitor(string stateFilePath)
        {
            StateFilePath = stateFilePath;

            // Vérifier si le fichier d'état existe déjà, sinon le créer
            if (!File.Exists(StateFilePath))
            {
                SetupStateFile();
            }
        }

        public void SetupStateFile()
        {
            try
            {
                // Créer un fichier JSON vide pour suivre l'état
                File.WriteAllText(StateFilePath, "{}");
                Console.WriteLine($"Fichier d'état créé : {StateFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du fichier d'état : {ex.Message}");
            }
        }

        public void UpdateState(BackupJob job, string status, int filesRemaining, long sizeRemaining)
        {
            try
            {
                // Objet représentant l'état actuel du travail
                var state = new
                {
                    JobName = job.Name,
                    SourceDirectory = job.SourceDirectory,
                    TargetDirectory = job.TargetDirectory,
                    BackupType = job.Type.ToString(),
                    Status = status,
                    FilesRemaining = filesRemaining,
                    SizeRemaining = sizeRemaining,
                    Timestamp = DateTime.Now
                };

                // Convertir l'objet en JSON
                string jsonState = JsonConvert.SerializeObject(state, Formatting.Indented);
                File.WriteAllText(StateFilePath, jsonState);

                Console.WriteLine($"État mis à jour pour le travail {job.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de l'état : {ex.Message}");
            }
        }
    }
}
