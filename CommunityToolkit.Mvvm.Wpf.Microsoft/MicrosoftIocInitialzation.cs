using CommunityToolkit.Mvvm.Wpf.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Windows;
namespace CommunityToolkit.Mvvm.DependencyInjection.Microsoft
{
    public abstract class MicrosoftIocInitialzation: Initialization
    {
       
        protected override IServiceProvider RegisterTypesAndBuilderIServiceProvider(IServiceCollection serviceDescriptors)
        {
            //serviceDescriptors.TryAddSingleton(serviceDescriptors);
            serviceDescriptors.TryAddSingleton<ISerivceProviderIsKeyedServiceType, ServiceInfo>();
           // serviceDescriptors.TryAddSingleton<IServiceProviderIsKeyedService, ServiceInfo>();
           // serviceDescriptors.TryAddSingleton<IServiceProviderIsService, ServiceInfo>();
            RegisterTypes(serviceDescriptors);
            return serviceDescriptors.BuildServiceProvider();
        }

        protected abstract void RegisterTypes(IServiceCollection serviceDescriptors);


        private class ServiceInfo : ISerivceProviderIsKeyedServiceType
        {
          //  private IServiceCollection _services;
            private IServiceProviderIsKeyedService _provider;
            public ServiceInfo(
                //IServiceCollection services,
                IServiceProviderIsKeyedService serviceProvider)
            {
                //_services = services;
                _provider= serviceProvider;
            }
            public Type? GetRegistrationType(string key)
            {
                return VMLocationProvider.Instance.ResolveViewType(key);
                //string key2 = key;
                //var serviceRegistrationInfo = (from r in _services
                //                               where r.IsKeyedService && key2.Equals(r.ServiceKey?.ToString(), StringComparison.Ordinal)
                //                               select r).FirstOrDefault();
                //if (serviceRegistrationInfo is null)
                //{
                //    serviceRegistrationInfo = (from r in _services
                //                               where r.IsKeyedService && key2.Equals(r.KeyedImplementationType.Name, StringComparison.Ordinal)
                //                               select r).FirstOrDefault();
                //}
                //return serviceRegistrationInfo?.KeyedImplementationType;

            }

            public bool IsKeyedService(Type serviceType, object? serviceKey)
            {
                return _provider.IsKeyedService(serviceType, serviceKey);
            }

            public bool IsService(Type serviceType)
            {
                return _provider.IsService(serviceType);
            }
        }
    }

}
