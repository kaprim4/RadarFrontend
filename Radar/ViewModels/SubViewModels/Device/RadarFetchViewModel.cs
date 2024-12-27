using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Radar.ViewModels.SubViewModels.Device
{
    public class RadarFetchViewModel : ViewModelBase
    {
        public ICommand FetchDataCommand { get; }
        public ICommand CancelCommand { get; }
        public RadarFetchViewModel()
        {

            FetchDataCommand = new ViewModelCommand(Fetch);
            CancelCommand = new ViewModelCommand(Cancel);
        }

        public void Fetch(object obj)
        {
            Window? window = Application.Current.Windows
                                   .OfType<Window>()
                                   .SingleOrDefault(w => w.DataContext == this);
            bool isOk = true;

            if (window != null)
            {
                window.DialogResult = isOk;
                window.Close();
            }
        }
        public void Cancel(object obj)
        {
            Window? window = Application.Current.Windows
                                   .OfType<Window>()
                                   .SingleOrDefault(w => w.DataContext == this);
            window?.Close();
        }

    }
}
