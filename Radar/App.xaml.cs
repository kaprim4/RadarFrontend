using ApiService;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Radar.Service;
using Radar.ViewModels;
using Radar.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DataView = Radar.Views.DataView;

namespace Radar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {

            IServiceCollection services = new ServiceCollection();

            services.AddTransient<LoginViewModel>();
            services.AddTransient(provider => new LoginView()
            {
                DataContext = provider.GetRequiredService<LoginViewModel>()
            });

            services.AddTransient<MainViewModel>();
            services.AddTransient(provider=> new MainView()
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddTransient<DashboardViewModel>();
            services.AddTransient(provider => new DashboardView()
            {
                DataContext = provider.GetRequiredService<DashboardViewModel>()
            });

            services.AddTransient<DevicesViewModel>();
            services.AddTransient(provider => new DevicesView()
            {
                DataContext = provider.GetRequiredService<DevicesViewModel>()
            });

            services.AddTransient<AddViewModel>();
            services.AddTransient(provider => new AddWindow()
            {
                DataContext = provider.GetRequiredService<AddViewModel>()
            });

            services.AddTransient<MembersViewModel>();
            services.AddTransient(provider => new MembersView()
            {
                DataContext = provider.GetRequiredService<MembersViewModel>()
            });

            services.AddTransient<DataViewModel>();
            services.AddTransient(provider => new DataView()
            {
                DataContext = provider.GetRequiredService<DataViewModel>()
            });

            services.AddTransient<AddDataViewModel>();
            services.AddTransient(provider => new AddDataWindow()
            {
                DataContext = provider.GetRequiredService<AddDataViewModel>()
            });

            services.AddTransient<DocumentDetailViewModel>();
            services.AddTransient(provider => new DocumentDetailView()
            {
                DataContext = provider.GetRequiredService<DocumentDetailViewModel>()
            });

            services.AddTransient<VehiculeDetailViewModel>();
            services.AddTransient(provider => new VehiculeDetailView()
            {
                DataContext = provider.GetRequiredService<VehiculeDetailViewModel>()
            });

            services.AddTransient<SettingViewModel>();
            services.AddTransient(provider => new SettingView()
            {
                DataContext = provider.GetRequiredService<SettingViewModel>()
            });

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IStateService, StateService>();
            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider=> viewModelType=> (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            services.AddSingleton<ISettingNavigationService, SettingNavigationService>();

            _serviceProvider = services.BuildServiceProvider();

        }



        protected override void OnStartup(StartupEventArgs e)
        {
            var loginView = _serviceProvider.GetRequiredService<LoginView>();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {

                    var mainView = _serviceProvider.GetRequiredService<MainView>();
                    mainView.Show();
                    loginView.Close();

                }
            };
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            base.OnStartup(e);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // put your tracing or logging code here (I put a message box as an example)
            MessageBox.Show(e.ExceptionObject.ToString());
        }


    }

}
