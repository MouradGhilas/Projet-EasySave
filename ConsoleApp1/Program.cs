using System;

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
    }
}