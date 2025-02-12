EasySave
Description

EasySave est une application de sauvegarde de fichiers développée en C#. Elle permet de créer et d'exécuter des travaux de sauvegarde, avec deux types de sauvegarde disponibles :

    Sauvegarde complète : Copie tous les fichiers du répertoire source vers le répertoire cible.

    Sauvegarde différentielle : Copie uniquement les fichiers modifiés ou ajoutés depuis la dernière sauvegarde.

L'application génère des logs détaillés au format JSON et enregistre l'état des sauvegardes en temps réel.
Fonctionnalités

    Création de jusqu'à 5 travaux de sauvegarde.

    Exécution manuelle ou automatique des sauvegardes.

    Support des répertoires locaux, externes et réseau.

    Logs journaliers au format JSON.

    État des sauvegardes en temps réel.

    Interface console conviviale.

Prérequis

    .NET Core SDK (version 5.0 ou supérieure).

    Un éditeur de texte ou un IDE (par exemple, Visual Studio, Visual Studio Code).

Installation

    Clone ce dépôt :
    bash
    Copy

    git clone https://github.com/ton-utilisateur/EasySave.git

    Accède au répertoire du projet :
    bash
    Copy

    cd EasySave

    Restaure les dépendances :
    bash
    Copy

    dotnet restore

    Compile le projet :
    bash
    Copy

    dotnet build

Utilisation

    Lance l'application :
    bash
    Copy

    dotnet run

    Utilise le menu interactif pour :

        Créer des travaux de sauvegarde.

        Lister les travaux existants.

        Exécuter une sauvegarde spécifique ou toutes les sauvegardes.

        Afficher les logs et l'état des sauvegardes.

Structure du projet
Copy

EasySave/
├── Enums/
│   └── BackupType.cs
├── Models/
│   └── BackupJob.cs
├── Services/
│   ├── BackupManager.cs
│   ├── Logger.cs
│   └── StateMonitor.cs
├── Program.cs
├── Livrable1test.csproj
├── README.md
└── Logs/
    └── 2023-10-25.json

Exemples de commandes
Créer un travail de sauvegarde

    Nom : Job1

    Répertoire source : C:\SourceFolder

    Répertoire cible : C:\TargetFolder

    Type : Complète

Exécuter une sauvegarde

    Exécuter toutes les sauvegardes : 4

    Exécuter une sauvegarde spécifique : 3 (puis entrer 1 pour exécuter Job1)

Afficher les logs

    Les logs sont enregistrés dans le dossier Logs au format JSON.

Format des logs

Les logs sont enregistrés dans un fichier JSON journalier (par exemple, 2023-10-25.json). Chaque entrée de log contient :

    Timestamp : Horodatage de l'action.

    BackupName : Nom de la sauvegarde.

    SourceFilePath : Chemin complet du fichier source.

    TargetFilePath : Chemin complet du fichier de destination.

    FileSize : Taille du fichier en octets.

    TransferTimeMs : Temps de transfert en millisecondes.

Exemple :
json
Copy

{
  "Timestamp": "2023-10-25T14:30:00",
  "BackupName": "Job1",
  "SourceFilePath": "C:\\SourceFolder\\file1.txt",
  "TargetFilePath": "C:\\TargetFolder\\file1.txt",
  "FileSize": 1024,
  "TransferTimeMs": 150
}

État des sauvegardes

L'état des sauvegardes est enregistré dans des fichiers JSON (par exemple, Job1_state.json). Chaque fichier contient :

    BackupName : Nom de la sauvegarde.

    LastActionTimestamp : Horodatage de la dernière action.

    Status : État du travail (Actif, Non Actif, Terminé).

    TotalFiles : Nombre total de fichiers éligibles.

    TotalSize : Taille totale des fichiers à transférer.

    RemainingFiles : Nombre de fichiers restants.

    RemainingSize : Taille des fichiers restants.

    CurrentSourceFile : Fichier source en cours de sauvegarde.

    CurrentTargetFile : Fichier de destination.

Exemple :
json
Copy

{
  "BackupName": "Job1",
  "LastActionTimestamp": "2023-10-25T14:30:00",
  "Status": "Actif",
  "TotalFiles": 10,
  "TotalSize": 1048576,
  "RemainingFiles": 5,
  "RemainingSize": 524288,
  "CurrentSourceFile": "C:\\SourceFolder\\file1.txt",
  "CurrentTargetFile": "C:\\TargetFolder\\file1.txt"
}

Contribuer

Les contributions sont les bienvenues ! Pour contribuer :

    Fork ce dépôt.

    Crée une branche pour ta fonctionnalité (git checkout -b feature/NouvelleFonctionnalité).

    Commit tes changements (git commit -m 'Ajouter une nouvelle fonctionnalité').

    Push vers la branche (git push origin feature/NouvelleFonctionnalité).

    Ouvre une Pull Request.

Auteurs

    CESI Team

Licence

Ce projet est sous licence MIT. Voir le fichier LICENSE pour plus de détails.