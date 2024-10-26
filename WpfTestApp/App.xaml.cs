using CommunityToolkit.Mvvm.DependencyInjection.DryIoc;
using System.Configuration;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using DryIoc;
using WpfTestApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        class MockBootstrapper : DryIocInitialzation
        {
            protected override void ConfigService(IServiceProvider serviceProvider)
            {

            }

            protected override DependencyObject CreateShell(IServiceProvider serviceProvider)
            {
                var mainwindow= serviceProvider.GetService<MainWindow>();
                return mainwindow;
            }

            protected override void RegisterTypes(DryIoc.IContainer serviceDescriptors)
            {
                serviceDescriptors.Register<TestControlA>(Reuse.Transient, serviceKey: "www");
                serviceDescriptors.BuilderViewAndViewModelByDryIoc(typeof(App).Assembly);
            }
        }
    }

}
