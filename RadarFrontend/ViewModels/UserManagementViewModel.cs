using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RadarFrontend.Models;
using RadarFrontend.Services;

namespace RadarFrontend.ViewModels
{
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly ApiService _apiService;

        public ObservableCollection<User> Users { get; set; }

        public UserManagementViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Users = new ObservableCollection<User>();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            var users = await _apiService.GetUsersAsync();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
}

