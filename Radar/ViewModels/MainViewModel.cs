using Domain.Models;
using Radar.Repository;
using Radar.Service;
using Radar.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged(nameof(NavigationService));
            }
        }

        private string _selectedView;
        public string SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged(nameof(SelectedView));
            }
        }
        //private object _currentView;
        //public object CurrentView
        //{
        //    get => _currentView;
        //    set
        //    {
        //        _currentView = value;
        //        OnPropertyChanged();
        //    }
        //}

        public bool IsRadaWindowVisible { get; set; }
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowDevicesCommand { get; }
        public ICommand ShowMembersCommand { get; }

        public ICommand ShowDataCommand { get; }
        public ICommand ShowWalletCommand { get; }
        public ICommand ShowMessagesCommand { get; }

        //private void Dashboard(object obj)
        //{
        //    CurrentView = new DashboardView();
        //    SelectedView = "Dashboard";
        //}
        //private void Members(object obj)
        //{
        //    CurrentView = new MembersView();
        //    SelectedView = "Members";
        //}

        //private void Data(object obj)
        //{
        //    CurrentView = new DataView();
        //    SelectedView = "Data";
        //}

        //private void Devices(object obj)
        //{
        //    CurrentView = new DevicesView();
        //    SelectedView = "Devices";
        //}
        //Fields
        private User _currentUserAccount;
        private IUserRepository userRepository;

        
        public User CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            // Set up commands
            ShowDashboardCommand = new ViewModelCommand(o => { NavigationService.NavigateTo<DashboardViewModel>(); }, o=> true);
            ShowMembersCommand = new ViewModelCommand(o => { NavigationService.NavigateTo<MembersViewModel>(); }, o => true);
            ShowDevicesCommand = new ViewModelCommand(o => { NavigationService.NavigateTo<DevicesViewModel>(); }, o => true);
            ShowDataCommand = new ViewModelCommand(o => { NavigationService.NavigateTo<DataViewModel>(); }, o => true);
            
            //ShowMembersCommand = new ViewModelCommand(Members);
            //ShowDevicesCommand = new ViewModelCommand(Devices);
            //ShowDataCommand = new ViewModelCommand(Data);
            userRepository = new UserRepository();
            CurrentUserAccount = new User();
            LoadCurrentUserData();




            //ShowMessagesCommand = new ViewModelCommand(_ => CurrentView = messagesViewModel);

          
                //CurrentView = new DashboardView();

            Task.Run(() => ShowRadarView());

            
        }


        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername("Othmane anouari");
            if (user != null)
            {
                CurrentUserAccount.UserName = user.UserName;
                CurrentUserAccount.FullName = $"Welcome {user.FullName};)";        
            }
            else
            {
                CurrentUserAccount.FullName = "Invalid user, not logged in";
            }
        }

        private void Border_MouseDown(object seneder, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //this.DragMove();
            }

        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object seneder, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    //this.WindowState = WindowState.Normal;
                    //this.Width = 1080;
                    //this.Height = 720;
                }
                else
                {
                    //this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }

        public async void ShowRadarView()
        {
            //while (true)
            //{
            //    if (IsRadaWindowVisible)
            //    {
            //            var RadarFetchWindow = new RadarFetchWindow();
            //            RadarFetchWindow.ShowDialog();
            //        IsRadaWindowVisible = false;
            //    }
            //    Thread.Sleep(500);
            //}
        }

        
    }
}
