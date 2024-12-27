using Domain.DTO;
using Microsoft.Win32;
using Radar.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Radar.Views
{
    /// <summary>
    /// Logique d'interaction pour MembresView.xaml
    /// </summary>
    public partial class SingleDataView : UserControl
    {
        public SingleDataView()
        {
            InitializeComponent(); 
        }
        private void TextBoxFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is DevicesViewModel viewModel)
                    viewModel.SearchFilter();
            }
        }


    }
}
