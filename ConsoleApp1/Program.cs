using System;
using System.Collections.Generic;
using System.IO;

class Task
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string title)
    {
        Title = title;
        IsCompleted = false;
    }

    public override string ToString()
    {
        return $"{Title} - {(IsCompleted ? "Fait" : "À faire")}";
    }
}

class TaskManager
{
    private List<Task> tasks;
    private string filePath = "todo.txt";
    private string backupFilePath = "backup_todo.txt";

    public TaskManager()
    {
        tasks = new List<Task>();
        LoadTasksFromFile();
    }

    public void AddTask(string title)
    {
        tasks.Add(new Task(title));
        SaveTasksToFile();
    }

    public void ViewTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Aucune tâche à afficher.");
            return;
        }

        Console.WriteLine("Tâches :");
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
    }

    public void MarkTaskAsCompleted(int taskNumber)
    {
        if (taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks[taskNumber - 1].IsCompleted = true;
            SaveTasksToFile();
        }
        else
        {
            Console.WriteLine("Numéro de tâche invalide.");
        }
    }

    public void DeleteTask(int taskNumber)
    {
        if (taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks.RemoveAt(taskNumber - 1);
            SaveTasksToFile();
        }
        else
        {
            Console.WriteLine("Numéro de tâche invalide.");
        }
    }

    private void SaveTasksToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.Title}|{task.IsCompleted}");
            }
        }

        // Sauvegarde automatique
        File.Copy(filePath, backupFilePath, true);
        Console.WriteLine("Sauvegarde automatique effectuée !");
    }

    private void LoadTasksFromFile()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    tasks.Add(new Task(parts[0]) { IsCompleted = bool.Parse(parts[1]) });
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();

        while (true)
        {
            Console.WriteLine("\n1. Ajouter une tâche");
            Console.WriteLine("2. Voir les tâches");
            Console.WriteLine("3. Marquer une tâche comme terminée");
            Console.WriteLine("4. Supprimer une tâche");
            Console.WriteLine("5. Quitter");
            Console.Write("Votre choix : ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Entrez le titre de la tâche : ");
                    string title = Console.ReadLine();
                    taskManager.AddTask(title);
                    break;

                case "2":
                    taskManager.ViewTasks();
                    break;

                case "3":
                    Console.Write("Entrez le numéro de la tâche à marquer comme terminée : ");
                    if (int.TryParse(Console.ReadLine(), out int markNumber))
                    {
                        taskManager.MarkTaskAsCompleted(markNumber);
                    }
                    else
                    {
                        Console.WriteLine("Entrée invalide.");
                    }
                    break;

                case "4":
                    Console.Write("Entrez le numéro de la tâche à supprimer : ");
                    if (int.TryParse(Console.ReadLine(), out int deleteNumber))
                    {
                        taskManager.DeleteTask(deleteNumber);
                    }
                    else
                    {
                        Console.WriteLine("Entrée invalide.");
                    }
                    break;

                case "5":
                    Console.WriteLine("Au revoir !");
                    return;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }
}
