using System.Windows;
using RadarFrontend.Services;
using RadarFrontend.ViewModels;

namespace RadarFrontend
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialiser le service API
            var apiService = new ApiService();

            // Créer l'instance du ViewModel principal
            var mainViewModel = new MainWindowViewModel();

            // Créer et afficher la fenêtre principale
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}
