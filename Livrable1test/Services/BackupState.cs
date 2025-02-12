using System;
using LoggerLibrary;

namespace Livrable1.Models
{
    public class BackupState
    {
        public string BackupName { get; set; } // Nom de la sauvegarde
        public DateTime LastActionTimestamp { get; set; } // Horodatage de la dernière action
        public string Status { get; set; } // État du travail (Actif, Non Actif, etc.)
        public int TotalFiles { get; set; } // Nombre total de fichiers éligibles
        public long TotalSize { get; set; } // Taille totale des fichiers à transférer
        public int RemainingFiles { get; set; } // Nombre de fichiers restants
        public long RemainingSize { get; set; } // Taille des fichiers restants
        public string CurrentSourceFile { get; set; } // Fichier source en cours de sauvegarde
        public string CurrentTargetFile { get; set; } // Fichier de destination
    }
}