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
        protected override IServiceProvider RegisterTypesAndBuilderIServiceProvider(IServiceCollection serviceDescriptors)
        {
            AddISerivceProviderIsKeyedServiceType(serviceDescriptors);
            var factory = new DryIocServiceProviderFactory();
            var ServiceProvider = factory.CreateBuilder(serviceDescriptors);
            RegisterTypes(ServiceProvider.Container);
            return ServiceProvider;
        }
        protected abstract void RegisterTypes(IContainer serviceDescriptors);

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
