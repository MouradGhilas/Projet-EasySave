using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenue dans un programme interactif en C#!");

        Console.Write("Entrez votre nom : ");
        string name = Console.ReadLine();

        Console.WriteLine($"Bonjour, {name} !");

        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"Ceci est le message numéro {i}");
        }

        // Appeler les nouvelles fonctionnalités
        ManipulerFichiers();
        GérerToDoList();
    }

    static void ManipulerFichiers()
    {
        string filePath = "example.txt";

        try
        {
            // Écriture dans un fichier
            File.WriteAllText(filePath, "Ceci est un fichier d'exemple.\n");

            // Ajouter des lignes supplémentaires
            File.AppendAllText(filePath, "Voici une ligne supplémentaire.\n");
            File.AppendAllText(filePath, "Encore une autre ligne ajoutée !\n");

            // Lecture depuis un fichier
            string content = File.ReadAllText(filePath);
            Console.WriteLine("Contenu du fichier :");
            Console.WriteLine(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur s'est produite lors de la manipulation du fichier : {ex.Message}");
        }
    }

    static void GérerToDoList()
    {
        string toDoFilePath = "todo.txt";
        List<string> toDoList = new List<string>();

        Console.WriteLine("\n=== Gestion de la To-Do List ===");

        // Charger les tâches existantes si le fichier existe
        if (File.Exists(toDoFilePath))
        {
            toDoList.AddRange(File.ReadAllLines(toDoFilePath));
            Console.WriteLine("Tâches existantes chargées depuis le fichier :");
            foreach (string task in toDoList)
            {
                Console.WriteLine($"- {task}");
            }
        }

        while (true)
        {
            Console.WriteLine("\nOptions :");
            Console.WriteLine("1 - Ajouter une tâche");
            Console.WriteLine("2 - Afficher les tâches");
            Console.WriteLine("3 - Supprimer une tâche");
            Console.WriteLine("4 - Quitter");

            Console.Write("Choisissez une option : ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Entrez une nouvelle tâche : ");
                    string newTask = Console.ReadLine();
                    toDoList.Add(newTask);
                    File.AppendAllText(toDoFilePath, newTask + "\n");
                    Console.WriteLine("Tâche ajoutée !");
                    break;

                case "2":
                    Console.WriteLine("Voici vos tâches actuelles :");
                    foreach (string task in toDoList)
                    {
                        Console.WriteLine($"- {task}");
                    }
                    break;

                case "3":
                    Console.WriteLine("Voici vos tâches actuelles :");
                    for (int i = 0; i < toDoList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} - {toDoList[i]}");
                    }

                    Console.Write("Entrez le numéro de la tâche à supprimer : ");
                    if (int.TryParse(Console.ReadLine(), out int taskNumber) &&
                        taskNumber > 0 && taskNumber <= toDoList.Count)
                    {
                        string removedTask = toDoList[taskNumber - 1];
                        toDoList.RemoveAt(taskNumber - 1);
                        File.WriteAllLines(toDoFilePath, toDoList);
                        Console.WriteLine($"Tâche supprimée : {removedTask}");
                    }
                    else
                    {
                        Console.WriteLine("Numéro de tâche invalide !");
                    }
                    break;

                case "4":
                    Console.WriteLine("Au revoir !");
                    return;

                default:
                    Console.WriteLine("Option invalide !");
                    break;
            }
        }
    }
}
