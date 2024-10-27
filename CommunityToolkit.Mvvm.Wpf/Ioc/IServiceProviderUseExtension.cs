using CommunityToolkit.Mvvm.Helper;
using CommunityToolkit.Mvvm.Modularity;
using CommunityToolkit.Mvvm.Navigation.Regions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public static class IServiceProviderUseExtension
    {

        public static IServiceProvider UseRegion<TWindow>(this IServiceProvider serviceProvider, TWindow window) where TWindow : DependencyObject
        {
            MvvmHelpers.AutowireViewModel(viewOrViewModel: window);
            RegionManager.SetRegionManager(window, serviceProvider.GetService<IRegionManager>());
            RegionManager.UpdateRegions();
            return serviceProvider;
        }

        public static IServiceProvider UseModule(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<ModuleManager>().OnInitialized(serviceProvider);
            return serviceProvider;
        }
    }
}
