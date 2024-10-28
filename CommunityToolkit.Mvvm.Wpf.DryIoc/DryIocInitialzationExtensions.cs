using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Example;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CommunityToolkit.Mvvm.DependencyInjection.DryIoc
{
    public abstract class DryIocInitialzation : Initialization
    {
        protected virtual IContainer BuilderContainer()
        {
            return new Container(DryIocAdapter.MicrosoftDependencyInjectionRules);
        }

        protected override IServiceProvider RegisterTypesAndBuilderIServiceProvider(IServiceCollection serviceDescriptors)
        {
            AddISerivceProviderIsKeyedServiceType(serviceDescriptors);
            var container = BuilderContainer();
            RegisterTypes(container, serviceDescriptors);
            var factory = new DryIocServiceProviderFactory(container);
            var ServiceProvider = factory.CreateBuilder(serviceDescriptors);
       
            return ServiceProvider;
        }
        protected abstract void RegisterTypes(IContainer container,IServiceCollection serviceDescriptors);

        protected void AddISerivceProviderIsKeyedServiceType(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<ServiceInfo>();
            serviceDescriptors.TryAddSingleton<ISerivceProviderIsKeyedServiceType>(p => p.GetService<ServiceInfo>());
        }
    }


    public static class DryIocExtensions
    {
        public static void BuilderViewAndViewModelByDryIoc(this IContainer container, params Assembly[] assemblies)
        {
            container.Populate(assemblies.SelectMany(x => x.RegisterViewAndViewModel()));
        }
    }
}
