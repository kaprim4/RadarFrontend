using Domain.DTO;
using Domain.Models;
using Radar.Repositories;
using Radar.Repository.Setting;
using Radar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class PathViewModel : ViewModelBase
    {
        private ISettingRepository _settingRepository;
        public ICommand SaveCommand { get; }
        private readonly IStateService _stateService;

        private string _inputDirectory;
        public string InputDirectory
        {
            get => _inputDirectory;
            set
            {
                _inputDirectory = value;
                OnPropertyChanged(nameof(InputDirectory));
            }
        }

        private string _outputDirectory;
        public string OutputDirectory
        {
            get => _outputDirectory;
            set
            {
                _outputDirectory = value;
                OnPropertyChanged(nameof(OutputDirectory));
            }
        }

        private string _treatedDirectory;
        public string TreatedDirectory
        {
            get => _treatedDirectory;
            set
            {
                _treatedDirectory = value;
                OnPropertyChanged(nameof(TreatedDirectory));
            }
        }

        private string _rejectedDirectory;
        public string RejectedDirectory
        {
            get => _rejectedDirectory;
            set
            {
                _rejectedDirectory = value;
                OnPropertyChanged(nameof(RejectedDirectory));
            }
        }

        private string _logDirectory;
        public string LogDirectory
        {
            get => _logDirectory;
            set
            {
                _logDirectory = value;
                OnPropertyChanged(nameof(LogDirectory));
            }
        }
        
        public PathViewModel(IStateService stateService)
        {
            _stateService = stateService;
            SaveCommand = new ViewModelCommand(Save);
            _settingRepository = new SettingRepository();
            GetSettingData();
        }

        public async void GetSettingData()
        {
            var data = await _settingRepository.List();
            if (data != null && (data.Pagable?.Content?.Any() ?? false))
            {
                var input = data.Pagable?.Content.FirstOrDefault(x=> x.Name == nameof(InputDirectory));
                if (input != null)
                {
                    InputDirectory = input.Value;
                }

                var output = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(OutputDirectory));
                if (output != null)
                {
                    OutputDirectory = output.Value;
                }

                var treated = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(TreatedDirectory));
                if (treated != null)
                {
                    TreatedDirectory = treated.Value;
                }

                var reject = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(RejectedDirectory));
                if (reject != null)
                {
                    RejectedDirectory = reject.Value;
                }

                var log = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(LogDirectory));
                if (log != null)
                {
                    LogDirectory = log.Value;
                }
            }
        }

        private async void Save(object obj)
        {
            var settings = new List<SettingDTO>()
            {
                new SettingDTO()
                {
                    Name = nameof(InputDirectory), Value = InputDirectory ?? ""
                },
                new SettingDTO()
                {
                    Name = nameof(OutputDirectory), Value = OutputDirectory ?? ""
                },
                new SettingDTO()
                {
                    Name = nameof(TreatedDirectory), Value = TreatedDirectory ?? ""
                },
                new SettingDTO()
                {
                    Name = nameof(RejectedDirectory), Value = RejectedDirectory ?? ""
                },
                new SettingDTO()
                {
                    Name = nameof(LogDirectory), Value = LogDirectory ?? ""
                },
            };

            var response = await _settingRepository.Update(settings);
            if (response.IsSuccess)
            {
                if(MessageBox.Show("Les chemain sont enregistrer avec success \n Mais pour le service les prendre en considiration il fait redemarer le service \n voulez-vous redémarer le service ? ", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var credentials = new ServerCredentials
                    {
                        isLocalHost = true
                    };

                    var appPoolNames = new[] { "JMXDecoder" };
                    await _stateService.RestartServiceAsync(credentials, "JMXDecoder");
                }
            }
        }
    }
}
