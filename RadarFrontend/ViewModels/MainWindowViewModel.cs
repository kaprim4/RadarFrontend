using System;
using System.Windows.Input;

namespace RadarFrontend.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateCommand { get; }

        public MainWindowViewModel()
        {
            // Initial View
            CurrentView = new DashboardViewModel();
            NavigateCommand = new RelayCommand(Navigate);
        }

        private void Navigate(object parameter)
        {
            switch (parameter)
            {
                case "Dashboard":
                    CurrentView = new DashboardViewModel();
                    break;
                case "UserManagement":
                    CurrentView = new UserManagementViewModel(new Services.ApiService());
                    break;
                case "DeviceManagement":
                    CurrentView = new DeviceManagementViewModel(new Services.ApiService());
                    break;
                case "Processing":
                    CurrentView = new ProcessingViewModel(new Services.ApiService());
                    break;
                default:
                    throw new ArgumentException("Invalid parameter for navigation");
            }
        }
    }
}

