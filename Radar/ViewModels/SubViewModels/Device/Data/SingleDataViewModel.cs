using Domain.DTO;
using Domain.Models;
using FileProcessor;
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
    public class SingleDataViewModel : PaggingViewModel<Data>
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
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
        public ICommand ReturnCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ViewDocumentCommand { get; }
        public ICommand AddDocumentCommand { get; }

        private ObservableCollection<string> _selectedFiles;

        private ObservableCollection<Document> _documentsGrid;


        public ObservableCollection<Document> DocumentsGrid
        {
            get => _documentsGrid;
            set
            {
                _documentsGrid = value;
                OnPropertyChanged(nameof(DocumentsGrid));
            }
        }
        public SingleDataViewModel(INavigationService navigationService, Lot lot) : base()
        {
            NavigationService = navigationService;
            ViewDocumentCommand = new GenericViewModelCommand<Document>(ViewDocument);
            DeleteCommand = new GenericViewModelCommand<Document>(DeleteDocument);
            ReturnCommand = new ViewModelCommand(Return);
            AddDocumentCommand = new GenericViewModelCommand<Document>(async d => await AddDocumentAsync(d));

            _dataRepository = new DataRepository();
            CurrentLot = lot;
            DocumentsGrid = new ObservableCollection<Document>(lot.Documents);
        }


        private void ViewDocument(Document document)
        {
            if (document == null)
                return;

            NavigationService.NavigateTo<DocumentDetailViewModel>(CurrentLot, document.Id);
        }

        private void Return(object obj)
        {
            NavigationService.NavigateTo<DataViewModel>();
        }

        private async Task AddDocumentAsync(Document document)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JMX Files (*.jmx)|*.jmx|All Files (*.*)|*.*",
                Multiselect = true,
                Title = "Select JMX Files"
            };

            // Always create and show dialogs on UI thread
            bool? dialogResult = await Application.Current.Dispatcher.InvokeAsync(() => dialog.ShowDialog());

            if (dialogResult != true) return;

            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() => IsLoading = true);
                var docs = dialog.FileNames.ToList();  // Now correctly accessing FileNames from the dialog object

                // Check for existing documents
                var existingDocs = CurrentLot.Documents
                    .Where(x => docs.Any(d => Path.GetFileName(d) == x.Name))
                    .Select(x => x.Name)
                    .ToList();

                if (existingDocs.Any())
                {
                    await Application.Current.Dispatcher.InvokeAsync(() => {
                        IsLoading = false;
                        var message = "les fichiers ci-dessous sont déjà traités : \n" +
                            existingDocs.Aggregate(string.Empty, (current, file) => current + (file + "\n")) +
                            "\n Voulez-vous traiter uniquement les fichiers non traités ? ";

                        var result = MessageBox.Show(message, "Alert", MessageBoxButton.YesNoCancel);

                        if (result == MessageBoxResult.Yes)
                        {
                            docs.RemoveAll(x => existingDocs.Contains(Path.GetFileName(x)));
                        }
                        else
                        {
                            return;
                        }
                        IsLoading = true;
                    });
                }

                foreach (var item in docs)
                {
                    var device = await Task.Run(() => JmxProcessor.GetDevice(item));
                    if (device == null)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() => {
                            IsLoading = false;
                            MessageBox.Show($"Impossible de lire le fichier : {Path.GetFileName(item)} !", "Alert");
                            IsLoading = true;
                        });
                        continue;
                    }

                    if (!CurrentLot.Documents.Any(x => x.Jmx.DeploymentSummary.CameraName == device))
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() => {
                            IsLoading = false;
                            MessageBox.Show($"Le fichier : {Path.GetFileName(item)} apartient à un autre Radar !", "Alert");
                            IsLoading = true;
                        });
                        continue;
                    }

                    var jmx = await Task.Run(() => JmxProcessor.DoWork(new string[] { item }, ""));
                    var newDocument = jmx
                        .Select(x => new JMX
                        {
                            DeploymentSummary = x.DeploymentSummary,
                            VehicleDatas = x.VehicleDatas
                        })
                        .Distinct()
                        .Select(j => new Document
                        {
                            Jmx = j,
                            Name = Path.GetFileName(item),
                            LotId = CurrentLot.Id
                        })
                        .FirstOrDefault();

                    if (newDocument != null)
                    {
                        var images = jmx.Select(x => x.Images).ToList();
                        await _dataRepository.AddDocument(newDocument);
                    }
                }

                await ReloadData();
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() => {
                    IsLoading = false;
                    MessageBox.Show($"Une erreur s'est produite: {ex.Message}", "Error");
                });
            }
            finally
            {
                await Application.Current.Dispatcher.InvokeAsync(() => IsLoading = false);
            }
        }



        private async void DeleteDocument(Document document)
        {
            if (document == null)
                return;
            if (MessageBox.Show("Are you sure you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _dataRepository.RemoveDocument(document.Id);
                await ReloadData();
            }
        }

        private async Task ReloadData()
        {
            CurrentLot = await _dataRepository.GetById(CurrentLot.Id);
            DocumentsGrid = new ObservableCollection<Document>(CurrentLot.Documents);
        }

        

       

    }
}
