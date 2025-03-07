using Radar.Repository;
using Radar.Service;
using Radar.Utilities;
using Radar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Radar.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IStateService _stateService;
        private readonly DispatcherTimer _refreshTimer;
        private readonly ObservableCollection<AppPoolStatus> _appPools;
        private string _iisStatus;
        private string _serviceStatus;
        private bool _isRefreshing;
        private ServerCredentials _credentials;



        //Constructor
        public DashboardViewModel(IStateService stateService)
        {
            _stateService = stateService;
            _appPools = new ObservableCollection<AppPoolStatus>();

            // Initialize timer
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(5) // Configurable refresh interval
            };
            _refreshTimer.Tick += async (s, e) => await RefreshDataAsync(null);

            InitializeAsync();
        }

        public ObservableCollection<AppPoolStatus> AppPools => _appPools;

        public string IISStatus
        {
            get => _iisStatus;
            private set
            {
                _iisStatus = value;
                OnPropertyChanged();
            }
        }

        public string ServiceStatus
        {
            get => _serviceStatus;
            private set
            {
                _serviceStatus = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            private set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand => new ViewModelCommand(async obj => await RefreshDataAsync(obj));
        public ICommand RestartIISCommand => new ViewModelCommand(RestartIISAsync);
        public ICommand RestartAppPoolCommand => new GenericViewModelCommand<string>(RestartAppPoolAsync);
        public ICommand RestartServiceCommand => new GenericViewModelCommand<string>(RestartServiceAsync);

        public async Task InitializeAsync()
        {


            //var credentials = new ServerCredentials
            //{
            //    ServerName = "10.2.4.51",
            //    Username = "PROCHECKIT\\srvadmin",
            //    Port = "5985",
            //    Password = SecureStringUtil.ConvertToSecureString("SYSPROD2023**"),
            //    UseSSL = false
            //};

            var credentials = new ServerCredentials
            {
                isLocalHost = true
            };

            var appPoolNames = new[] { "JMXDecoder" };

            _credentials = credentials;

            foreach (var appPoolName in appPoolNames)
            {
                _appPools.Add(new AppPoolStatus { Name = appPoolName });
            }

            await RefreshDataAsync(null);
            _refreshTimer.Start();
        }

        private async Task RefreshDataAsync(object obj)
        {
            try
            {
                IsRefreshing = true;

                // Update IIS Status
                IISStatus = await _stateService.CheckIISStatusAsync(_credentials);
                ServiceStatus = await _stateService.CheckServiceAsync(_credentials, "JMXDecoder");

                // Update App Pool Statuses
                foreach (var appPool in _appPools)
                {
                    //appPool.Status = await _stateService.CheckAppPoolStatusAsync(_credentials, appPool.Name);
                    var tt  = await _stateService.CheckAppPoolStatusAsync(_credentials, appPool.Name);
                    appPool.LastChecked = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void RestartIISAsync(object obj)
        {
            try
            {
                await _stateService.RestartIISAsync(_credentials);
                await RefreshDataAsync(null);
            }
            catch (Exception ex)
            {
                // Handle errors
                Debug.WriteLine($"Error restarting IIS: {ex.Message}");
            }
        }

        private async void RestartAppPoolAsync(string appPoolName)
        {
            try
            {
                await _stateService.RestartAppPoolAsync(_credentials, appPoolName);
                await RefreshDataAsync(null);
            }
            catch (Exception ex)
            {
                // Handle errors
                Debug.WriteLine($"Error restarting app pool: {ex.Message}");
            }
        }

        private async void RestartServiceAsync(string appPoolName)
        {
            try
            {
                await _stateService.RestartServiceAsync(_credentials, "JMXDecoder");
                await RefreshDataAsync(null);
            }
            catch (Exception ex)
            {
                // Handle errors
                Debug.WriteLine($"Error restarting app pool: {ex.Message}");
            }
        }



    }
}
