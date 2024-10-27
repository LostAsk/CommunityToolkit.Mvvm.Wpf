using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Helper;
using CommunityToolkit.Mvvm.Modularity;
using CommunityToolkit.Mvvm.Navigation.Regions;
using CommunityToolkit.Mvvm.Navigation.Regions.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public abstract class Initialization
    {
        protected DependencyObject Shell { get;private set; }

        protected IServiceProvider ServiceProvider => Ioc.Default;

 
        public void Run()
        {
           
            Initialize();
            OnInitialized();
        }
        protected virtual void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                return Ioc.Default.GetService(type);
            });
        }
        private void Initialize()
        {
            var service = new ServiceCollection();
            RegisterRequiredTypes(service);
            RegisterRequiredTypesBehavior(service);
            var service_provider = RegisterTypesAndBuilderIServiceProvider(service);
            ConfigPrismSerivce(service_provider);
            ConfigService(service_provider);
        }

        protected abstract IServiceProvider RegisterTypesAndBuilderIServiceProvider(IServiceCollection serviceDescriptors);

        protected virtual void ConfigService(IServiceProvider serviceProvider)
        {
            serviceProvider.UseModule();
        }
        private void ConfigurePrismService(IServiceProvider serviceProvider)
        {
          
            var behaviorFactory = serviceProvider.GetService<IRegionBehaviorFactory>();
            RegisterDefaultRegionBehaviors(behaviorFactory);
            var regionAdapterMappings = serviceProvider.GetService<RegionAdapterMappings>();
            ConfigureRegionAdapterMappings(regionAdapterMappings);

        }
        private void RegisterRequiredTypes(IServiceCollection serviceDescriptors)
        {
           
            serviceDescriptors.TryAddSingleton<RegionAdapterMappings>();
            serviceDescriptors.TryAddSingleton<IRegionManager, RegionManager>();
            serviceDescriptors.TryAddSingleton<IRegionNavigationContentLoader, RegionNavigationContentLoader>();
            serviceDescriptors.TryAddSingleton<IRegionBehaviorFactory, RegionBehaviorFactory>();
            serviceDescriptors.TryAddTransient<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>();
            serviceDescriptors.TryAddTransient<IRegionNavigationJournal, RegionNavigationJournal>();
            serviceDescriptors.TryAddTransient<IRegionNavigationService, RegionNavigationService>();
            serviceDescriptors.TryAddSingleton<DelayedRegionCreationBehavior>();
            serviceDescriptors.TryAddSingleton<SelectorRegionAdapter>();
            serviceDescriptors.TryAddSingleton<ItemsControlRegionAdapter>();
            serviceDescriptors.TryAddSingleton<ContentControlRegionAdapter>();
            serviceDescriptors.AddDialog();
            var module_manger = new ModuleManager();
            //添加模块管理器
            serviceDescriptors.TryAddSingleton<ModuleManager>(module_manger);
            ConfigModule(module_manger);
            module_manger.ConfigModuleService(serviceDescriptors);
        }

        protected virtual void ConfigModule(ModuleManager moduleManager)
        {

        }

        private void RegisterRequiredTypesBehavior(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.TryAddTransient<BindRegionContextToDependencyObjectBehavior>();
            serviceDescriptors.TryAddTransient<RegionActiveAwareBehavior>();
            serviceDescriptors.TryAddTransient<SyncRegionContextWithHostBehavior>();
            serviceDescriptors.TryAddTransient<RegionManagerRegistrationBehavior>();
            serviceDescriptors.TryAddTransient<RegionMemberLifetimeBehavior>();
            serviceDescriptors.TryAddTransient<ClearChildViewsRegionBehavior>();
            serviceDescriptors.TryAddTransient<DestructibleRegionBehavior>();
        }


        protected void ConfigPrismSerivce(IServiceProvider serviceProvider)
        {
            Ioc.Default.ConfigureServices(serviceProvider);
            ConfigurePrismService(serviceProvider);
            ConfigureViewModelLocator();
            var window = CreateShell(serviceProvider);
            if (window != null)
            {
                serviceProvider.UseRegion(window);
                InitializeShell(window);
            }
            //OnInitialized();
             
        }

       

        protected virtual void RegisterDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing<BindRegionContextToDependencyObjectBehavior>(BindRegionContextToDependencyObjectBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<RegionActiveAwareBehavior>(RegionActiveAwareBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<SyncRegionContextWithHostBehavior>(SyncRegionContextWithHostBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<RegionManagerRegistrationBehavior>(RegionManagerRegistrationBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<RegionMemberLifetimeBehavior>(RegionMemberLifetimeBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<ClearChildViewsRegionBehavior>(ClearChildViewsRegionBehavior.BehaviorKey);
            regionBehaviors.AddIfMissing<DestructibleRegionBehavior>(DestructibleRegionBehavior.BehaviorKey);
        
        }
        protected virtual void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping<Selector, SelectorRegionAdapter>();
            regionAdapterMappings.RegisterMapping<ItemsControl, ItemsControlRegionAdapter>();
            regionAdapterMappings.RegisterMapping<ContentControl, ContentControlRegionAdapter>();
        }


        protected abstract DependencyObject CreateShell(IServiceProvider serviceProvider);
        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected virtual void InitializeShell(DependencyObject shell)
        {
            Shell = shell;
        }

        /// <summary>
        /// Contains actions that should occur last.
        /// </summary>
        protected virtual void OnInitialized()
        {
            if (Shell is Window window)
                window.Show();
        }


    }
}
