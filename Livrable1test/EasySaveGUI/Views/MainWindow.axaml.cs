using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;


namespace EasySaveGUI.Views
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> BackupJobs { get; set; } = new ObservableCollection<string>();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AddJob(object sender, EventArgs e)
        {
            BackupJobs.Add("Nouveau travail de sauvegarde");
        }

        private async void RemoveJob(object sender, EventArgs e)
        {
            if (BackupJobs.Count > 0)
            {
                var result = await ShowConfirmationDialog("Supprimer", "Voulez-vous vraiment supprimer ce travail ?");
                if (result)
                {
                    BackupJobs.RemoveAt(BackupJobs.Count - 1);
                }
            }
        }

        private async void ExecuteJob(object sender, EventArgs e)
        {
            if (BackupJobs.Count > 0)
            {
                var result = await ShowConfirmationDialog("Exécuter", $"Voulez-vous exécuter le travail : {BackupJobs[0]} ?");
                if (result)
                {
                    Console.WriteLine($"Exécution du travail : {BackupJobs[0]}");
                    BackupJobs.RemoveAt(0);
                    await ShowMessageDialog("Succès", "Le travail a été exécuté avec succès.");
                }
            }
        }

        private async Task<bool> ShowConfirmationDialog(string title, string message)
        {
            var dialog = new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = message },
                        new Button { Content = "Oui", Name = "YesButton" },
                        new Button { Content = "Non", Name = "NoButton" }
                    }
                },
                Width = 300,
                Height = 150
            };

            var tcs = new TaskCompletionSource<bool>();

            ((Button)((StackPanel)dialog.Content).Children[1]).Click += (_, _) => { tcs.SetResult(true); dialog.Close(); };
            ((Button)((StackPanel)dialog.Content).Children[2]).Click += (_, _) => { tcs.SetResult(false); dialog.Close(); };

            dialog.Show();
            return await tcs.Task;
        }

        private async Task ShowMessageDialog(string title, string message)
        {
            var dialog = new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = message },
                        new Button { Content = "OK", Name = "OkButton" }
                    }
                },
                Width = 300,
                Height = 150
            };

            var tcs = new TaskCompletionSource<bool>();
            ((Button)((StackPanel)dialog.Content).Children[1]).Click += (_, _) => { tcs.SetResult(true); dialog.Close(); };

            dialog.Show();
            await tcs.Task;
        }
    }
}