using Domain.DTO;
using Domain.Models;
using Radar.Repository.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public abstract class PaggingViewModel<T> : ViewModelBase where T : class
    {
        public int _currentPage { get; set; }
        public int _totalPages { get; set; }
        public string _searchValue { get; set; }
        public bool _showEllipsis { get; set; }
        public int _selectedPage { get; set; }
        public int _dataNumbers;
        public PagableDTO<T> _pagination = new();
        public ObservableCollection<int> _pages;

        public ICommand GoToPreviousPage => new ViewModelCommand(GoToPreviousPageAction);
        public ICommand GoToNextPage => new ViewModelCommand(GoToNextPageAction);
        public ICommand GoToPageCommand => new GenericViewModelCommand<int>(GoToPageAction);
        public ICommand PageClickCommand { get; }


        public PaggingViewModel()
        {
            Pages = new ObservableCollection<int>();
            PageClickCommand = new ViewModelCommand(OnPageClick);
        }

        public void Initialize()
        {
            LoadData();
        }

        public async Task InitializeAsync()
        {
            await LoadDataAsync();
        }

        public virtual void LoadData()
        {
            throw new NotImplementedException();
        }

        public async virtual Task LoadDataAsync()
        {
            throw new NotImplementedException();
        }

        public string SearchValue
        {
            get => _searchValue;
            set
            {
                _searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }


        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                UpdatePages(_totalPages);
            }
        }

        public bool ShowEllipsis
        {
            get => _showEllipsis;
            set
            {
                _showEllipsis = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<int> Pages
        {
            get => _pages;
            set
            {
                _pages = value;
                OnPropertyChanged();
            }
        }

        public int DataNumbers
        {
            get => _dataNumbers;
            set
            {
                _dataNumbers = value;
                OnPropertyChanged(nameof(DataNumbers));
            }
        }



        public void UpdatePages(int totalPages)
        {
            _totalPages = totalPages;

            Pages.Clear();
            for (int i = 1; i <= totalPages; i++)
                Pages.Add(i);


            ShowEllipsis = totalPages > 7;
        }


        public void GoToPreviousPageAction(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        public void GoToNextPageAction(object obj)
        {
            if (CurrentPage < _totalPages)
            {
                CurrentPage++;
            }
        }

        public void GoToPageAction(int pageNumber)
        {
            CurrentPage = pageNumber;
        }


        public void OnPageClick(object pageNumber)
        {
            if (pageNumber != null)
            {
                SelectedPage = (int)pageNumber;
                _pagination.Page = (int)pageNumber;
                LoadData();
            }
        }

        public int SelectedPage
        {
            get => _selectedPage;
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    OnPropertyChanged(nameof(SelectedPage));
                }
            }
        }
    }
}
