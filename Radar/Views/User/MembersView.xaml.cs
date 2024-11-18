using Radar.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MembersView : UserControl
    {
        public ICommand SelectionChangedCommand { get; private set; }


        public MembersView()
        {
            InitializeComponent(); 
            DataContext = new MembersViewModel();
            var converter = new BrushConverter();
            //ObservableCollection<Member> members = new ObservableCollection<Member>();

            //members.Add(new Member { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098AD"), Name = "John Doe", Position = "Coach", Email = "john.doe@gmail.com", Phone = "415-954-1475" });

            //membersDataGrid.ItemsSource = members;
        }


        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
