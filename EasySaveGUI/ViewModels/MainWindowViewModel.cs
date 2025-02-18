using ReactiveUI;
using System;
using Avalonia.Controls;

namespace EasySaveGUI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private object _selectedMenuItem;

        public object SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
                HandleMenuSelection(value);
            }
        }

        private void HandleMenuSelection(object selectedItem)
        {
            if (selectedItem is ListBoxItem listBoxItem)
            {
                string menuChoice = listBoxItem.Content.ToString();

                switch (menuChoice)
                {
                    case "Créer un travail de sauvegarde":
                        Console.WriteLine("Accueil sélectionné !");
                        break;

                    case "Lister les travaux de sauvegarde":
                        Console.WriteLine("Sauvegarde sélectionnée !");
                        break;

                    case "Exécuter une sauvegarde":
                        Console.WriteLine("Paramètres sélectionnés !");
                        break;
                    
                    case "Exécuter toutes les sauvegardes":
                        Console.WriteLine("Paramètres sélectionnés !");
                        break;

                    case "Afficher les logs":
                        Console.WriteLine("Paramètres sélectionnés !");
                        break;

                    case "Quitter":
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
