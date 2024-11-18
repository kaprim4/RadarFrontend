using Domain.Models;
using Radar.Repository;
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
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowEventsCommand { get; }
        public ICommand ShowMembersCommand { get; }
        public ICommand ShowWalletCommand { get; }
        public ICommand ShowMessagesCommand { get; }

        private void Dashboard(object obj)
        {
            CurrentView = new DashboardView();
            SelectedView = "Dashboard";
        }
        private void Members(object obj)
        {
            CurrentView = new MembersView();
            SelectedView = "Members";
        }
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

        public MainViewModel()
        {
            // Set up commands
            ShowDashboardCommand = new ViewModelCommand(Dashboard);
            ShowMembersCommand = new ViewModelCommand(Members);

            userRepository = new UserRepository();
            CurrentUserAccount = new User();
            LoadCurrentUserData();


           
            //ShowWalletCommand = new ViewModelCommand(_ => CurrentView = walletViewModel);
            //ShowMessagesCommand = new ViewModelCommand(_ => CurrentView = messagesViewModel);


            CurrentView = new DashboardView();
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
    }
}
