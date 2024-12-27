using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Domain.Models;
using ApiService;
using Domain.DTO;
using Radar.Helper;
using Radar.Repositories;
using Radar.Repository.Device;
using Radar.Service;

namespace Radar.ViewModels
{
    public class AddViewModel : ViewModelBase
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
        private IRepositoryBase<Device> _deviceRepository;
        private string _name;
        private string _serialNumber;
        private bool _isActive;
        public bool IsUpdate = false;
        public int Id = 0;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string SerialNumber
        {
            get => _serialNumber;
            set { _serialNumber = value; OnPropertyChanged(nameof(SerialNumber)); }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value; OnPropertyChanged(nameof(IsActive));
                }
                    
            }
        }

       

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            _deviceRepository = new DeviceRepository();
            SaveCommand = new ViewModelCommand(Save);
            CancelCommand = new ViewModelCommand(Cancel);

        }
        public AddViewModel(INavigationService navigationService, Device? device)
        {
            NavigationService = navigationService;
            _deviceRepository = new DeviceRepository();
            SaveCommand = new ViewModelCommand(Save);
            CancelCommand = new ViewModelCommand(Cancel);

            if (device != null)
            {
                Id = device.Id;
                Name = device.Name;
                SerialNumber = device.SerialNumber;
                IsActive = device.IsActive;
            }
            
            IsUpdate = device != null;
        }



        private async void Save(object obj)
        {
            var device = new Device()
            {
                IsActive = IsActive,
                Name = Name,
                SerialNumber = SerialNumber
            };

            ResponseModel<Device> response = new();

            if (IsUpdate)
            {
                device.Id = Id;
                response = await _deviceRepository.Edit(device);
            }
            else
                response =  await _deviceRepository.Add(device);

            NavigationService.NavigateTo<DevicesViewModel>();
        }

        private void Cancel(object obj)
        {
            NavigationService.NavigateTo<DevicesViewModel>();
        }

        
    }
}
