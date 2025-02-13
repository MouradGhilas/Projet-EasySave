EasySave
Description
EasySave is a file backup application developed in C#. It allows creating and executing backup tasks with two available backup types:

Full Backup: Copies all files from the source directory to the target directory.
Differential Backup: Copies only the files that have been modified or added since the last backup.
The application generates detailed logs in JSON format and records the real-time status of backups.

Features
Create up to 5 backup tasks.
Manual or automatic execution of backups.
Support for local, external, and network directories.
Daily logs in JSON format.
Real-time backup status tracking.
User-friendly console interface.
Prerequisites
.NET Core SDK (version 5.0 or higher).
A text editor or IDE (e.g., Visual Studio, Visual Studio Code).
Installation
Clone this repository:

bash
Copier
Modifier
git clone https://github.com/your-username/EasySave.git
Navigate to the project directory:

bash
Copier
Modifier
cd EasySave
Restore dependencies:

bash
Copier
Modifier
dotnet restore
Build the project:

bash
Copier
Modifier
dotnet build
Usage
Run the application:

bash
Copier
Modifier
dotnet run
Use the interactive menu to:

Create backup tasks.
List existing tasks.
Execute a specific backup or all backups.
View logs and backup statuses.
Project Structure
markdown
Copier
Modifier
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
Command Examples
Create a backup task
Name: Job1
Source directory: C:\SourceFolder
Target directory: C:\TargetFolder
Type: Full
Execute a backup
Execute all backups: 4
Execute a specific backup: 3 (then enter 1 to execute Job1)
View logs
Logs are stored in the Logs folder in JSON format.
Log Format
Logs are recorded in a daily JSON file (e.g., 2023-10-25.json). Each log entry contains:

Timestamp: Action timestamp.
BackupName: Name of the backup task.
SourceFilePath: Full path of the source file.
TargetFilePath: Full path of the target file.
FileSize: Size of the file in bytes.
TransferTimeMs: File transfer time in milliseconds.
Example:
json
Copier
Modifier
{
  "Timestamp": "2023-10-25T14:30:00",
  "BackupName": "Job1",
  "SourceFilePath": "C:\\SourceFolder\\file1.txt",
  "TargetFilePath": "C:\\TargetFolder\\file1.txt",
  "FileSize": 1024,
  "TransferTimeMs": 150
}
Backup Status
Backup statuses are saved in JSON files (e.g., Job1_state.json). Each file contains:

BackupName: Name of the backup task.
LastActionTimestamp: Timestamp of the last action.
Status: Job status (Active, Not Active, Completed).
TotalFiles: Total number of eligible files.
TotalSize: Total size of files to transfer.
RemainingFiles: Number of remaining files.
RemainingSize: Size of remaining files.
CurrentSourceFile: Current source file being backed up.
CurrentTargetFile: Current target file.
Example:
json
Copier
Modifier
{
  "BackupName": "Job1",
  "LastActionTimestamp": "2023-10-25T14:30:00",
  "Status": "Active",
  "TotalFiles": 10,
  "TotalSize": 1048576,
  "RemainingFiles": 5,
  "RemainingSize": 524288,
  "CurrentSourceFile": "C:\\SourceFolder\\file1.txt",
  "CurrentTargetFile": "C:\\TargetFolder\\file1.txt"
}
Contribute
Contributions are welcome! To contribute:

Fork this repository.
Create a branch for your feature (git checkout -b feature/NewFeature).
Commit your changes (git commit -m 'Add a new feature').
Push to the branch (git push origin feature/NewFeature).
Open a Pull Request.
Authors
CESI Team
License
This project is licensed under the MIT License. See the LICENSE file for details.
