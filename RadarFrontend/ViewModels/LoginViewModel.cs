using RadarFrontend.Services;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace RadarFrontend.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ApiService _apiService;
        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private async void ExecuteLogin(object parameter)
        {
            try
            {
                var token = await _apiService.Authenticate(Username, Password);
                MessageBox.Show("Connexion réussie !");
                // Naviguer vers le tableau de bord
            }
            catch (Exception ex)
            {
                MessageBox.Show("Échec de la connexion : " + ex.Message);
            }
        }
    }
}
