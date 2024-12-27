using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Radar.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Open(Type view, object[]? args, Action<object>? openAction)
        {
            if (view == null || !typeof(Window).IsAssignableFrom(view))
                throw new ArgumentException("The view must be a Window type", nameof(view));

            Window? window;

            if (args != null && args.Any())
            {
                window = Activator.CreateInstance(view, args) as Window;
            }
            else
            {
                window = Activator.CreateInstance(view) as Window;
            }

            if (window == null)
                throw new InvalidOperationException("Failed to create an instance of the window.");

            // Execute the action if provided
            openAction?.Invoke(window);

            // Show the window
            window.Show();
        }

    }
}
