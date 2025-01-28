using System;
using System.Collections.Generic;
using System.IO;

class Task
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string description)
    {
        Description = description;
        IsCompleted = false;
    }

    public override string ToString()
    {
        return $"{(IsCompleted ? "[X]" : "[ ]")} {Description}";
    }
}

class TaskManager
{
    private List<Task> tasks = new List<Task>();
    private string filePath = "todo.txt";

    public TaskManager()
    {
        LoadTasksFromFile();
    }

    public void AddTask(string description)
    {
        tasks.Add(new Task(description));
        SaveTasksToFile();
    }

    public void ViewTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Aucune tâche disponible.");
            return;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
    }

    public void MarkTaskAsCompleted(int index)
    {
        if (index >= 0 && index < tasks.Count)
        {
            tasks[index].IsCompleted = true;
            SaveTasksToFile();
        }
        else
        {
            Console.WriteLine("Index invalide.");
        }
    }

    public void DeleteTask(int index)
    {
        if (index >= 0 && index < tasks.Count)
        {
            tasks.RemoveAt(index);
            SaveTasksToFile();
        }
        else
        {
            Console.WriteLine("Index invalide.");
        }
    }

    private void LoadTasksFromFile()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    Task task = new Task(parts[0]);
                    task.IsCompleted = bool.Parse(parts[1]);
                    tasks.Add(task);
                }
            }
        }
    }

    private void SaveTasksToFile()
    {
        List<string> lines = new List<string>();
        foreach (Task task in tasks)
        {
            lines.Add($"{task.Description}|{task.IsCompleted}");
        }
        File.WriteAllLines(filePath, lines);
    }
}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Ajouter une tâche");
            Console.WriteLine("2. Voir les tâches");
            Console.WriteLine("3. Marquer une tâche comme terminée");
            Console.WriteLine("4. Supprimer une tâche");
            Console.WriteLine("5. Quitter");
            Console.Write("Votre choix : ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Entrez la description de la tâche : ");
                    string description = Console.ReadLine();
                    taskManager.AddTask(description);
                    break;
                case "2":
                    taskManager.ViewTasks();
                    break;
                case "3":
                    Console.Write("Entrez le numéro de la tâche à marquer comme terminée : ");
                    if (int.TryParse(Console.ReadLine(), out int completedIndex))
                    {
                        taskManager.MarkTaskAsCompleted(completedIndex - 1);
                    }
                    break;
                case "4":
                    Console.Write("Entrez le numéro de la tâche à supprimer : ");
                    if (int.TryParse(Console.ReadLine(), out int deleteIndex))
                    {
                        taskManager.DeleteTask(deleteIndex - 1);
                    }
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
    }
}
