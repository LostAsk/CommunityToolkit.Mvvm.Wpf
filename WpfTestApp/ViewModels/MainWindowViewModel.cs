using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Navigation.Regions;
using Microsoft.Extensions.DependencyInjection;
using WpfTestApp.Views;
namespace WpfTestApp.ViewModels
{
    internal partial class MainWindowViewModel: ObservableObject
    {
        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            var p = serviceProvider.GetService<IKeyedServiceProvider>();
            var k= serviceProvider.GetService<IServiceProviderIsKeyedService>();
            var k1 = serviceProvider.GetService<IServiceProviderIsService>();
            var k2 = serviceProvider.GetService<ISerivceProviderIsKeyedServiceType>();
            var k3 = serviceProvider.GetService<IServiceProvider>();
            var jjj = k3.GetKeyedService<TestControlA>("www");
            var jjj2 = k3.GetKeyedService<TestControlA>("TestControlA");
        }
        /// <summary>
        /// 标题
        /// </summary>
        [ObservableProperty]
        string title;

        [RelayCommand]
        void Loadoo(object par)
        {
            
            var view = Ioc.Default.GetService<TestControlA>();
            Ioc.Default.GetService<IRegionManager>().AddToRegion("user", view);
            //ServiceProvider.Default.GetService<IRegionManager>().Regions["user"].Activate(view);
        }
    }
}
