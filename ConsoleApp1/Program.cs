using System;
using System.IO;

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
    }

    static void ManipulerFichiers()
    {
        string filePath = "example.txt";

        // Écriture dans un fichier
        File.WriteAllText(filePath, "Ceci est un fichier d'exemple.");

        // Lecture depuis un fichier
        string content = File.ReadAllText(filePath);
        Console.WriteLine("Contenu du fichier : " + content);
    }
}
