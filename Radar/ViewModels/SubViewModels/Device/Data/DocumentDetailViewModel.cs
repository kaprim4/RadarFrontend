using Domain.DTO;
using Domain.Models;
using Microsoft.Win32;
using Radar.Repositories;
using Radar.Repository.Data;
using Radar.Service;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class DocumentDetailViewModel : PaggingViewModel<Data>
    {

        private Lot _currentLot;
        public Lot CurrentLot
        {
            get => _currentLot;
            set
            {
                _currentLot = value;
                OnPropertyChanged(nameof(CurrentLot));
            }
        }
        private VehicleData _currentVehicle;
        public VehicleData CurrentVehicle
        {
            get => _currentVehicle;
            set
            {
                _currentVehicle = value;
                OnPropertyChanged(nameof(CurrentVehicle));
            }
        }

        private string _currentImage;
        public string CurrentImage
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        private JMX _currentDocument;
        public JMX CurrentDocument
        {
            get => _currentDocument;
            set
            {
                _currentDocument = value;
                OnPropertyChanged(nameof(CurrentDocument));
            }
        }
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
        private IRepositoryBase<Lot> _dataRepository;
        public ICommand EditCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PrevCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand DeleteCommand { get; }
        private ObservableCollection<string> _selectedFiles;

        private ObservableCollection<VehicleData> _vehicleGrid;


        public ObservableCollection<VehicleData> VehicleGrid
        {
            get => _vehicleGrid;
            set
            {
                _vehicleGrid = value;
                OnPropertyChanged(nameof(VehicleGrid));
            }
        }

        public DocumentDetailViewModel(INavigationService navigationService, Lot lot, int id) : base()
        {
            NavigationService = navigationService;
            EditCommand = new GenericViewModelCommand<VehicleData>(ViewVehicle);
            ReturnCommand = new ViewModelCommand(o=>{NavigationService.NavigateTo<SingleDataViewModel>(lot);}, o=> true);
            NextCommand = new ViewModelCommand(NextVehiculeData);
            PrevCommand = new ViewModelCommand(PrevVehiculeData);
            ViewCommand = new GenericViewModelCommand<VehicleData>(ViewVehicle);
            CurrentLot = lot;

            _dataRepository = new DataRepository();
            CurrentDocument = lot.Documents.FirstOrDefault(x => x.Id == id) == null ? new JMX()
                : lot.Documents.FirstOrDefault(x => x.Id == id).Jmx;

            //VehicleGrid = CurrentDocument?.VehicleDatas != null ? new ObservableCollection<VehicleData>(CurrentDocument.VehicleDatas)
            //    : new ObservableCollection<VehicleData>();

            CurrentDocument.VehicleDatas =  CurrentDocument.VehicleDatas.OrderBy(x => x.Id).ToList();
            CurrentVehicle = CurrentDocument.VehicleDatas.FirstOrDefault();
            CurrentImage = Path.Combine(Environment.CurrentDirectory, "Images", Path.GetFileName(CurrentVehicle.Jmx), CurrentVehicle.Image);
        }


        private void ViewVehicle(VehicleData vehicle)
        {
            if (vehicle == null)
                return;

            NavigationService.NavigateTo<VehiculeDetailViewModel>(_currentLot, CurrentDocument.Id, vehicle.Id);
        }


        private async void PrevVehiculeData(object ob)
        {
            if (CurrentVehicle != null)
            {
                var index = CurrentDocument.VehicleDatas.IndexOf(CurrentVehicle);

                CurrentVehicle = index > 0 ? CurrentDocument.VehicleDatas[index - 1] : CurrentDocument.VehicleDatas[CurrentDocument.VehicleDatas.Count - 1];
                CurrentImage = Path.Combine(Environment.CurrentDirectory, "Images", Path.GetFileName(CurrentVehicle.Jmx), CurrentVehicle.Image);
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }
        private async void NextVehiculeData(object obj)
        {
            if (CurrentVehicle != null)
            {
                var index = CurrentDocument.VehicleDatas.IndexOf(CurrentVehicle);

                CurrentVehicle = index < CurrentDocument.VehicleDatas.Count - 1 ? CurrentDocument.VehicleDatas[index + 1] : CurrentDocument.VehicleDatas[0];
                CurrentImage = Path.Combine(Environment.CurrentDirectory, "Images", Path.GetFileName(CurrentVehicle.Jmx), CurrentVehicle.Image);
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }

       

    }
}
