using Radar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private ISettingNavigationService _navigationService;
        public ISettingNavigationService SettingNavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged(nameof(SettingNavigationService));
            }
        }

        public ICommand ShowPathsCommand { get; }
        public ICommand ShowSuperviserCommand { get; }

        public SettingViewModel(ISettingNavigationService settingNavigationService)
        {
            SettingNavigationService = settingNavigationService;
            ShowPathsCommand = new ViewModelCommand(o => { SettingNavigationService.NavigateTo<PathViewModel>(); }, o => true);

            SettingNavigationService.NavigateTo<PathViewModel>();
        }
    }
}
