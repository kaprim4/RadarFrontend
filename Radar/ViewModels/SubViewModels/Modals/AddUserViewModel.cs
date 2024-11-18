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

namespace Radar.ViewModels.SubViewModels
{
    public class AddUserViewModel : ViewModelBase
    {
        private IProcess<User> _process = new("auth");
        private string _username;
        private string _password;
        private string _email;
        private string _phone;
        private string _fullName;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        public AddUserViewModel()
        {
            SaveCommand = new ViewModelCommand(Save, CanSave);
            CancelCommand = new ViewModelCommand(Cancel);
        }


        private bool CanSave(object obj) => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);

        private async void Save(object obj)
        {
            // Close the dialog and set DialogResult to true
            //DialogResult = true;
            var user = new User()
            {
                UserName = Username,
                Email = Email,
                FullName = FullName,
                Phone = Phone,
                Password = Password
            };
            var response = await _process.ProcessAsync(user, RequestType.Post, EndPoint.register, true, TokenManager.JwtToken);
            if (response != null && response.IsSuccess)
            {
                var window = Application.Current.Windows
                                     .OfType<Window>()
                                     .SingleOrDefault(w => w.DataContext == this);

                if (window != null)
                {
                    window.DialogResult = true; // Ensure the window can be closed by setting DialogResult.
                    window.Close(); // Close the dialog.
                }
            }
            //DialogResult = true;
        }

        private void Cancel(object obj)
        {
            var window = Application.Current.Windows
                                     .OfType<Window>()
                                     .SingleOrDefault(w => w.DataContext == this);

            if (window != null)
            {
                window.DialogResult = false; // Ensure the window can be closed by setting DialogResult.
                window.Close(); // Close the dialog.
            }


        }
    }
}
