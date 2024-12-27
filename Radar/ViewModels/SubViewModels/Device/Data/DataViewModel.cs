using Domain.DTO;
using Domain.Models;
using Microsoft.Win32;
using Radar.Repositories;
using Radar.Repository.Data;
using Radar.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class DataViewModel : PaggingViewModel<Data>
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
        private IDataRepository _dataRepository;
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand SelectFilesCommand { get; }
        private ObservableCollection<string> _selectedFiles;

        private ObservableCollection<Data> _dataDataGrid;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ObservableCollection<string> SelectedFiles
        {
            get => _selectedFiles;
            set
            {
                _selectedFiles = value;
                OnPropertyChanged(nameof(SelectedFiles));
            }
        }

        public ObservableCollection<Data> dataDataGrid
        {
            get => _dataDataGrid;
            set
            {
                _dataDataGrid = value;
                OnPropertyChanged(nameof(dataDataGrid));
            }
        }

        public DataViewModel(INavigationService navigationService, PagableDTO<Data>? pagableDTO = null) : base()
        {
            ViewCommand = new GenericViewModelCommand<Data>(ViewLot);
            EditCommand = new GenericViewModelCommand<Data>(EditDevice);
            DeleteCommand = new GenericViewModelCommand<Data>(DeleteLot);
            SelectedFiles = new ObservableCollection<string>();
            SelectFilesCommand = new GenericViewModelCommand<object>(async obj => await SelectFilesAsync(obj));
            NavigationService = navigationService;

            _dataRepository = new DataRepository();

            if (pagableDTO != null)
            {
                _pagination = pagableDTO;
                SearchValue = pagableDTO.SearchTerm;
            }

            InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            try
            {
                IsLoading = true;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}", "Erreur"));
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override async Task LoadDataAsync()
        {
            try
            {
                var lots = await _dataRepository.GetAllGenerique<Data>(_pagination);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    dataDataGrid = lots?.Pagable?.Content?.Any() ?? false
                        ? new ObservableCollection<Data>(lots.Pagable.Content)
                        : new ObservableCollection<Data>();

                    DataNumbers = lots?.Pagable?.TotalContent ?? 0;
                    if (DataNumbers != 0)
                    {
                        int pages = (DataNumbers + _pagination.Length - 1) / _pagination.Length;
                        UpdatePages(pages);
                    }
                });
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}", "Erreur"));
            }
        }

        private void EditDevice(Data device)
        {
            //var addWindow = new AddWindow(device);
            //if (addWindow.ShowDialog() == true)
            //    LoadData();
        }

        private async void DeleteLot(Data data)
        {
            if (data == null) return;

            var result = await Application.Current.Dispatcher.InvokeAsync(() =>
                MessageBox.Show("Voulez vous vraiment continuer ?", "Confirmation",
                    MessageBoxButton.YesNo, MessageBoxImage.Question));

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    var response = await _dataRepository.Remove(data.Id);
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                        MessageBox.Show($"Erreur lors de la suppression: {ex.Message}", "Erreur"));
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private async void ViewLot(Data data)
        {
            if (data == null) return;

            try
            {
                IsLoading = true;
                var lot = await _dataRepository.GetById(data.Id);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (lot != null)
                    {
                        NavigationService.NavigateTo<SingleDataViewModel>(lot);
                    }
                    else
                    {
                        MessageBox.Show("Impossible de récupérer les détails de l'élément sélectionné.",
                            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Erreur lors de la récupération des détails: {ex.Message}", "Erreur"));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SelectFilesAsync(object obj)
        {
            try
            {
                IsLoading = true;
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "JMX Files (*.jmx)|*.jmx|All Files (*.*)|*.*",
                    Multiselect = true,
                    Title = "Select JMX Files"
                };

                bool? result = await Application.Current.Dispatcher.InvokeAsync(() => dialog.ShowDialog());

                if (result != true) return;

                SelectedFiles.Clear();
                foreach (var file in dialog.FileNames)
                {
                    SelectedFiles.Add(file);
                }
                var filesArray = SelectedFiles.ToList();

                var checkFiles = await _dataRepository.ChechFiles<List<string>>(
                    filesArray.Select(x => Path.GetFileName(x)).ToList());

                if (checkFiles.Any(x => !x.CanTreat))
                {
                    var messageResult = await Application.Current.Dispatcher.InvokeAsync(() =>
                        MessageBox.Show("Les fichiers ci-dessous sont déjà traités : \n"
                            + checkFiles.Where(x => !x.CanTreat).Select(x => x.Name)
                                .Aggregate(string.Empty, (current, file) => current + (file + "\n"))
                            + " \n Voulez-vous traiter uniquement les fichiers non traités ? ",
                            "Alert", MessageBoxButton.YesNoCancel));

                    if (messageResult == MessageBoxResult.Yes)
                    {
                        filesArray = filesArray.Where(x => !checkFiles
                            .Where(c => !c.CanTreat)
                            .Select(c => c.Name)
                            .Contains(Path.GetFileName(x)))
                            .ToList();
                    }
                    else
                    {
                        return;
                    }
                }

                if (filesArray.Any())
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        NavigationService.NavigateTo<AddDataViewModel>(filesArray);
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Une erreur s'est produite: {ex.Message}", "Error"));
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}