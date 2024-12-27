using Domain.DTO;
using Domain.Models;
using Microsoft.Win32;
using Radar.Repositories;
using Radar.Repository.Data;
using Radar.Service;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class VehiculeDetailViewModel : PaggingViewModel<Data>
    {

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
        public ICommand ReturnCommand { get; }

        public VehiculeDetailViewModel(INavigationService navigationService, Lot lot, int idDocument, int idVehicule) : base()
        {
            NavigationService = navigationService;
            ReturnCommand = new ViewModelCommand(o=>{NavigationService.NavigateTo<DocumentDetailViewModel>(lot, lot.Documents.FirstOrDefault(x => x.Jmx.Id == idDocument).Id);}, o=> true);


            CurrentVehicle = lot.Documents.FirstOrDefault(x => x.Jmx.Id == idDocument) == null ? new VehicleData()
                : lot.Documents.FirstOrDefault(x => x.Jmx.Id == idDocument).Jmx.VehicleDatas.FirstOrDefault(x=>x.Id == idVehicule);

        }


       

    }
}
