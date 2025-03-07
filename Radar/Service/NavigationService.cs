using Microsoft.Extensions.DependencyInjection;
using Radar.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Radar.Service
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        void NavigateTo<T>(params object[] parameters) where T : ViewModelBase;
    }
    public class NavigationService : ViewModelBase, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<Type, ViewModelBase> _viewModelBaseFactory;
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public NavigationService(Func<Type, ViewModelBase> viewModelBaseFactory, IServiceProvider serviceProvider)
        {
            _viewModelBaseFactory = viewModelBaseFactory;
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>(params object[]? parameters) where TViewModel : ViewModelBase
        {
            try
            {
                var viewModelType = typeof(TViewModel);
                var resolvedParameters = parameters ?? Array.Empty<object>();

                // Ensure we're on the UI thread
                if (!Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() => NavigateTo<TViewModel>(parameters));
                    return;
                }

                // Create the ViewModel instance with error handling
                ViewModelBase viewModel;
                try
                {
                    viewModel = (ViewModelBase)ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, resolvedParameters);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating ViewModel: {ex}");
                    throw;
                }

                // Ensure the old view is cleaned up
                if (_currentView != null)
                {
                    try
                    {
                        (_currentView as IDisposable)?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error disposing old view: {ex}");
                    }
                }

                // Set the new view with error handling
                try
                {
                    CurrentView = viewModel;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting CurrentView: {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation error: {ex}");
                throw;
            }
        }
    }
    public interface ISettingNavigationService
    {
        ViewModelBase CurrentView { get; }
        void NavigateTo<T>(params object[] parameters) where T : ViewModelBase;
    }

    public class SettingNavigationService : ViewModelBase, ISettingNavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<Type, ViewModelBase> _viewModelBaseFactory;
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public SettingNavigationService(Func<Type, ViewModelBase> viewModelBaseFactory, IServiceProvider serviceProvider)
        {
            _viewModelBaseFactory = viewModelBaseFactory;
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>(params object[]? parameters) where TViewModel : ViewModelBase
        {
            try
            {
                var viewModelType = typeof(TViewModel);
                var resolvedParameters = parameters ?? Array.Empty<object>();

                // Ensure we're on the UI thread
                if (!Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() => NavigateTo<TViewModel>(parameters));
                    return;
                }

                // Create the ViewModel instance with error handling
                ViewModelBase viewModel;
                try
                {
                    viewModel = (ViewModelBase)ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType, resolvedParameters);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating ViewModel: {ex}");
                    throw;
                }

                // Ensure the old view is cleaned up
                if (_currentView != null)
                {
                    try
                    {
                        (_currentView as IDisposable)?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error disposing old view: {ex}");
                    }
                }

                // Set the new view with error handling
                try
                {
                    CurrentView = viewModel;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting CurrentView: {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation error: {ex}");
                throw;
            }
        }
    }
}
