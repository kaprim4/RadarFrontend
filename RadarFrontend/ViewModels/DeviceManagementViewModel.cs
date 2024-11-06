using System.Collections.ObjectModel;
using RadarFrontend.Models;
using RadarFrontend.Services;

namespace RadarFrontend.ViewModels
{
    public class DeviceManagementViewModel : ViewModelBase
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Device> Devices { get; set; }

        public DeviceManagementViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Devices = new ObservableCollection<Device>();
            LoadDevices();
        }

        private async void LoadDevices()
        {
            var devices = await _apiService.GetDevicesAsync();
            foreach (var device in devices)
            {
                Devices.Add(device);
            }
        }
    }
}
