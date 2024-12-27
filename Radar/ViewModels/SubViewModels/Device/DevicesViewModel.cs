using Domain.DTO;
using Domain.Models;
using Radar.Repositories;
using Radar.Repository;
using Radar.Repository.Device;
using Radar.Service;
using Radar.Views;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Radar.ViewModels
{
    public class DevicesViewModel : PaggingViewModel<Device>
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
        public ICommand OpenAddDialogCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ViewCommand { get; }




        private IRepositoryBase<Device> _deviceRepository;
        private ObservableCollection<Device> _devicesDataGrid;
        private bool _isBackdropVisible;
        
       
        

        

        public ObservableCollection<Device> devicesDataGrid
        {
            get => _devicesDataGrid;
            set
            {
                _devicesDataGrid = value;
                OnPropertyChanged(nameof(devicesDataGrid));
            }
        }
        public bool IsBackdropVisible
        {
            get => _isBackdropVisible;
            set
            {
                _isBackdropVisible = value;
                OnPropertyChanged(nameof(IsBackdropVisible));
            }
        }

        

        

        //Constructor
        public DevicesViewModel(INavigationService navigationService) : base()
        {
            NavigationService = navigationService;
            _deviceRepository = new DeviceRepository();
            OpenAddDialogCommand = new ViewModelCommand(OpenAddDialog);
            ViewCommand = new GenericViewModelCommand<Device>(ViewLot);
            EditCommand = new GenericViewModelCommand<Device>(EditDevice);
            DeleteCommand = new GenericViewModelCommand<Device>(DeleteDevice);

            Initialize();
        }

        public override async void LoadData()
        {
            //var converter = new BrushConverter();
            var members = await _deviceRepository.GetAll(_pagination);
            members.Pagable.Content.ForEach(d =>
            {
                d.BgColor = d.IsActive ? "green" : "red";
            });
            devicesDataGrid = members != null && members.Pagable != null && members.Pagable.Content.Any() ?
                new ObservableCollection<Device>(members.Pagable.Content) :
                new ObservableCollection<Device>() ;

            DataNumbers = members?.Pagable?.TotalContent ?? 0 ;

            int pages = (DataNumbers % _pagination.Length == 0)
                ? DataNumbers / _pagination.Length
                : (DataNumbers / _pagination.Length) + 1;
            UpdatePages(pages);
        }

        private void OpenAddDialog(object obj)
        {
            NavigationService.NavigateTo<AddViewModel>();

        }

        private void EditDevice(Device device)
        {
            NavigationService.NavigateTo<AddViewModel>(device);
        }

        private void ViewLot(Device device)
        {
            NavigationService.NavigateTo<DataViewModel>(new PagableDTO<Data>()
            {
                SearchTerm = device.SerialNumber
            });
        }

        private async void DeleteDevice(Device device)
        {
            if (device == null)
                return;
            if (MessageBox.Show("Are you sure you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var response = await _deviceRepository.Remove(device.SerialNumber);
                LoadData();
            }
            
            
        }

        public void SearchFilter()
        {
            _pagination.SearchTerm = SearchValue;
            LoadData();
        }

        
    }
}
