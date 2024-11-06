using System.Collections.ObjectModel;
using RadarFrontend.Models;
using RadarFrontend.Services;

namespace RadarFrontend.ViewModels
{
    public class ProcessingViewModel : ViewModelBase
    {
        private readonly ApiService _apiService;
        public ObservableCollection<string> ProcessedData { get; set; }

        public ProcessingViewModel(ApiService apiService)
        {
            _apiService = apiService;
            ProcessedData = new ObservableCollection<string>();
            LoadProcessedData();
        }

        private async void LoadProcessedData()
        {
            // Exemple fictif : Charger les données depuis l'API
            ProcessedData.Add("Donnée traitée 1");
            ProcessedData.Add("Donnée traitée 2");
            // Ajoutez ici l'appel réel à l'API pour obtenir les données de traitement
        }
    }
}
