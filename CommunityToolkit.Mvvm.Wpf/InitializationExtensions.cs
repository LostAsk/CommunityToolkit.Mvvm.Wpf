using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Navigation.Regions;
using CommunityToolkit.Mvvm.Navigation.Regions.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public static class InitializationExtensions
    {
        private  static void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                return Ioc.Default.GetService(type);
            });
        }

        private static void ConfigurePrismService(IServiceProvider serviceProvider)
        {
          
            var behaviorFactory = serviceProvider.GetService<IRegionBehaviorFactory>();
            behaviorFactory.RegisterDefaultRegionBehaviors();
            var regionAdapterMappings = serviceProvider.GetService<RegionAdapterMappings>();
            regionAdapterMappings.RegisterDefaultRegionAdapterMappings();

        }
        public static void RegisterRequiredTypes(this IServiceCollection serviceDescriptors,Action<IServiceCollection> builder_service)
        {
           
            serviceDescriptors.TryAddSingleton<RegionAdapterMappings>();
            serviceDescriptors.TryAddSingleton<IRegionManager, RegionManager>();
            serviceDescriptors.TryAddSingleton<IRegionNavigationContentLoader, RegionNavigationContentLoader>();
            serviceDescriptors.TryAddSingleton<IRegionBehaviorFactory, RegionBehaviorFactory>();
            serviceDescriptors.TryAddTransient<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>();
            serviceDescriptors.TryAddTransient<IRegionNavigationJournal, RegionNavigationJournal>();
            serviceDescriptors.TryAddTransient<IRegionNavigationService, RegionNavigationService>();
            serviceDescriptors.TryAddSingleton<SelectorRegionAdapter>();
            serviceDescriptors.TryAddSingleton<ItemsControlRegionAdapter>();
            serviceDescriptors.TryAddSingleton<ContentControlRegionAdapter>();
            builder_service?.Invoke(serviceDescriptors);
        
        }

        public static IServiceProvider ConfigPrismSerivce(this IServiceProvider serviceProvider)
        {
            Ioc.Default.ConfigureServices(serviceProvider);
            ConfigurePrismService(serviceProvider);
           
            ConfigureViewModelLocator();
            return serviceProvider;
        }

        private static void RegisterDefaultRegionBehaviors(this IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing<BindRegionContextToDependencyObjectBehavior>(BindRegionContextToDependencyObjectBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<RegionActiveAwareBehavior>(RegionActiveAwareBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<SyncRegionContextWithHostBehavior>(SyncRegionContextWithHostBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<RegionManagerRegistrationBehavior>(RegionManagerRegistrationBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<RegionMemberLifetimeBehavior>(RegionMemberLifetimeBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<ClearChildViewsRegionBehavior>(ClearChildViewsRegionBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<DestructibleRegionBehavior>(DestructibleRegionBehavior.BehaviorKey);
        
        }

        private static void RegisterDefaultRegionAdapterMappings(this RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping<Selector, SelectorRegionAdapter>();
            regionAdapterMappings.RegisterMapping<ItemsControl, ItemsControlRegionAdapter>();
            regionAdapterMappings.RegisterMapping<ContentControl, ContentControlRegionAdapter>();
        }
    }
}
