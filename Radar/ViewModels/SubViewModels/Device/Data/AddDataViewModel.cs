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
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Microsoft.Win32;
using FileProcessor;
using Radar.Repository.Data;
using Radar.Service;

namespace Radar.ViewModels
{
    public class AddDataViewModel : PaggingViewModel<File>
    {
        private INavigationService _navigationService;
        private readonly IRepositoryBase<Lot> _dataRepository;
        private bool _isProcessing;

        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged(nameof(NavigationService));
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteFileCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddFileCommand { get; }

        private ObservableCollection<File> _files;
        public ObservableCollection<File> Files
        {
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }

        private string _nLot;
        public string NLot
        {
            get => _nLot;
            set
            {
                _nLot = value;
                OnPropertyChanged(nameof(NLot));
            }
        }

        private string _createdDate;
        public string CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged(nameof(CreatedDate));
            }
        }

        private int _nbrFiles;
        public int NbrFiles
        {
            get => _nbrFiles;
            set
            {
                _nbrFiles = value;
                OnPropertyChanged(nameof(NbrFiles));
            }
        }

        public AddDataViewModel(INavigationService navigationService, List<string> files) : base()
        {
            NavigationService = navigationService;
            _dataRepository = new DataRepository();
            Files = new ObservableCollection<File>();

            SaveCommand = new ViewModelCommand(async (obj) => await SaveAsync(obj));
            CancelCommand = new ViewModelCommand(Cancel);
            AddFileCommand = new ViewModelCommand(async (obj) => await AddFile(obj));
            DeleteFileCommand = new GenericViewModelCommand<File>(DeleteItem);
            InitializeWithFiles(files);
        }

        private void InitializeWithFiles(List<string> files)
        {
            if (files != null && files.Any())
            {
                int count = 1;
                foreach (var item in files)
                {
                    Files.Add(new File()
                    {
                        Count = count++,
                        Name = item
                    });
                }

                NLot = GenerateLotRefrence();
                CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                NbrFiles = Files.Count;
            }
        }

        private async Task SaveAsync(object obj)
        {
            if (IsProcessing) return;

            try
            {
                IsProcessing = true;

                if (!Files.Any())
                {
                    await ShowMessageAsync("Aucun fichier à traiter.");
                    return;
                }

                List<DeploymentData> deploymentData;
                try
                {
                    deploymentData = await Task.Run(() =>
                        JmxProcessor.DoWork(Files.Select(x => x.Name).ToArray(), ""));
                }
                catch (DivideByZeroException)
                {
                    await ShowMessageAsync("Erreur lors du traitement des fichiers: Division par zéro détectée. Veuillez vérifier vos données d'entrée.");
                    return;
                }
                catch (Exception ex)
                {
                    await ShowMessageAsync($"Erreur lors du traitement des fichiers: {ex.Message}");
                    return;
                }

                if (!await ValidateDeploymentData(deploymentData))
                {
                    return;
                }

                var lot = CreateLot(deploymentData);
                if (lot.Documents == null || !lot.Documents.Any())
                {
                    await ShowMessageAsync("Impossible de créer des documents valides à partir des fichiers.");
                    return;
                }

                try
                {
                    await _dataRepository.Add(lot);
                    await NavigateToDataView();
                }
                catch (Exception ex)
                {
                    await ShowMessageAsync($"Erreur lors de la sauvegarde: {ex.Message}");
                }
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private void Cancel(object obj)
        {
            NavigateToDataView();
        }

        private void DeleteItem(File file)
        {
            if (file == null) return;

            Files.Remove(file);
            NbrFiles = Files.Count;

            if (!Files.Any())
            {
                NavigateToDataView();
            }
        }

        private async Task AddFile(object obj)
        {
            if (IsProcessing) return;

            try
            {
                IsProcessing = true;
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "JMX Files (*.jmx)|*.jmx|All Files (*.*)|*.*",
                    Multiselect = true,
                    Title = "Select JMX Files"
                };

                bool? result = await Application.Current.Dispatcher.InvokeAsync(() =>
                    dialog.ShowDialog());

                if (result != true) return;

                var lastCount = Files.Count + 1;
                foreach (var file in dialog.FileNames)
                {
                    if (!Files.Any(x => x.Name == file))
                    {
                        Files.Add(new File()
                        {
                            Count = lastCount++,
                            Name = file
                        });
                    }
                }

                NbrFiles = Files.Count;
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"Une erreur s'est produite: {ex.Message}");
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private async Task<bool> ValidateDeploymentData(List<DeploymentData> deploymentData)
        {
            if (deploymentData == null || !deploymentData.Any())
            {
                await ShowMessageAsync("Aucune donnée n'a été extraite des fichiers.");
                return false;
            }

            var serialNumbers = deploymentData
                .Where(x => x?.DeploymentSummary != null)
                .Select(x => x.DeploymentSummary.CameraName)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();

            if (!serialNumbers.Any())
            {
                await ShowMessageAsync("Aucun numéro de série valide n'a été trouvé.");
                return false;
            }

            return true;
        }

        private Lot CreateLot(List<DeploymentData> deploymentData)
        {
            return new Lot
            {
                Reference = NLot,
                Documents = deploymentData
                    .Where(x => x?.DeploymentSummary != null && x?.VehicleDatas != null)
                    .Select(x => new JMX
                    {
                        DeploymentSummary = x.DeploymentSummary,
                        VehicleDatas = x.VehicleDatas
                    })
                    .Where(x => x.VehicleDatas != null && x.VehicleDatas.Any())
                    .Distinct()
                    .Select(jmx => new Document
                    {
                        Jmx = jmx,
                        Name = jmx.VehicleDatas.FirstOrDefault()?.Jmx ??
                               System.IO.Path.GetFileName(Files.FirstOrDefault()?.Name)
                    })
                    .ToList()
            };
        }

        private async Task ShowMessageAsync(string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show(message, "Alert"));
        }

        private async Task NavigateToDataView()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                NavigationService.NavigateTo<DataViewModel>();
            });
        }

        private string GenerateLotRefrence()
        {
            return $"L-xxxxxx-{DateTime.Now:ddMMyy}";
        }
    }
}