using Radar.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {

        private IUserRepository userRepository;

        

        //Constructor
        public DashboardViewModel()
        {
            userRepository = new UserRepository();
            //LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        //private bool CanExecuteLoginCommand(object obj)
        //{
        //    bool validData;
        //    if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
        //        Password == null || Password.Length < 3)
        //        validData = false;
        //    else
        //        validData = true;
        //    return validData;
        //}

        //private async void ExecuteLoginCommand(object obj)
        //{
        //    var isValidUser = await userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
        //    if (isValidUser)
        //    {
        //        Thread.CurrentPrincipal = new GenericPrincipal(
        //            new GenericIdentity(Username), null);
        //        IsViewVisible = false;
        //    }
        //    else
        //    {
        //        ErrorMessage = "* Invalid username or password";
        //    }
        //}

        //private void ExecuteRecoverPassCommand(string username, string email)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
