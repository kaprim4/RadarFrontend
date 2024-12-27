using Domain.DTO;
using Domain.Models;
using Radar.Repository;
using Radar.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Radar.ViewModels
{
    public class MembersViewModel : ViewModelBase
    {
        public ICommand OpenAddUserDialogCommand { get; }
        private IUserRepository _userRepository;
        private ObservableCollection<User> _membersDataGrid;
        private bool _isBackdropVisible;
        private string _searchValue;
        private int _userNumbers;

        private PagableDTO<User> _pagination = new();

        public ObservableCollection<User> membersDataGrid
        {
            get => _membersDataGrid;
            set
            {
                _membersDataGrid = value;
                OnPropertyChanged(nameof(membersDataGrid));
            }
        }
        public bool IsBackdropVisible
        {
            get => _isBackdropVisible;
            set
            {
                _isBackdropVisible = value;
                OnPropertyChanged(nameof(IsBackdropVisible));
            }
        }

        public int UserNumbers
        {
            get => _userNumbers;
            set
            {
                _userNumbers = value;
                OnPropertyChanged(nameof(UserNumbers));
            }
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



        private string[] colors = { "#1098AD", "#1E88E5", "#FF8F00", "#FF5252", "#0CA678", "#6741D9", "#FF6D00", "#FF5252", "#1E88E5", "#0CA678" };

        //Constructor
        public MembersViewModel()
        {
            _userRepository = new UserRepository();
            OpenAddUserDialogCommand = new ViewModelCommand(OpenAddUserDialog);
            LoadMembers();
        }

        private async void LoadMembers()
        {
            var converter = new BrushConverter();
            var members = await _userRepository.GetAll(_pagination);
            if (members != null)
            {
                if (members.Pagable != null && members.Pagable.Content.Any())
                {
                    foreach (var item in members.Pagable.Content)
                    {
                        int random = Random.Shared.Next(0, 9);
                        item.BgColor = (Brush)converter.ConvertFromString(colors[random]);
                        item.Character = item.FullName[..1];
                        item.Order++;
                    }
                    UserNumbers = members.Pagable.TotalContent;
                    membersDataGrid = new ObservableCollection<User>(members.Pagable.Content);
                }
                else
                {
                    membersDataGrid = new ObservableCollection<User>();
                }


            }



        }


        private void OpenAddUserDialog(object obj)
        {
            IsBackdropVisible = true;
            var addUserWindow = new AddUserWindow();
            if (addUserWindow.ShowDialog() == true)
                LoadMembers();

        }


        public void SearchFilter()
        {
            _pagination.SearchTerm = SearchValue;
            LoadMembers();
        }

        
    }
}
