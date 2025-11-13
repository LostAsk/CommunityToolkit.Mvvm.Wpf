using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection.Microsoft;
using CommunityToolkit.Mvvm.Wpf.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WpfTestApp.Views;
namespace WpfTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var boot = new MockBootstrapper();
            boot.Run();
        }

        class MockBootstrapper : MicrosoftIocInitialzation
        {
            protected override void ConfigureViewModelLocator()
            {
                VMLocationProvider.Instance.SetServiceProvider(Ioc.Default);
                VMLocationProvider.Instance
                    .SetDefaultViewModelFactory(ServiceCollectionViewBindVmExtensions.UseWpfTestAppDefaultViewModelFatory)
                    .SetDefaultViewFactory(ServiceCollectionViewBindVmExtensions.UseWpfTestAppDefaultViewFatory)
                    ;

                ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
                {
                    return ServiceCollectionViewBindVmExtensions.UseWpfTestAppDefaultViewModelFatory(view, Ioc.Default);
                });

               
            }

            protected override void ConfigService(IServiceProvider serviceProvider)
            {

            }

            protected override DependencyObject CreateShell(IServiceProvider serviceProvider)
            {
                var mainwindow= serviceProvider.GetService<MainWindow>();
                return mainwindow;
            }

            protected override void RegisterTypes(IServiceCollection serviceDescriptors)
            {
                serviceDescriptors.AddWpfTestAppViewAndViewModel();
            }
        }
    }

}
