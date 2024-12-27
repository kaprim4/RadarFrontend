using Radar.Repository;
using Radar.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ICommand OpenRadarFetchCommand { get; }


        //Constructor
        public DashboardViewModel()
        {
            //OpenRadarFetchCommand = new ViewModelCommand(OpenRadarFetch);
        }



    }
}
