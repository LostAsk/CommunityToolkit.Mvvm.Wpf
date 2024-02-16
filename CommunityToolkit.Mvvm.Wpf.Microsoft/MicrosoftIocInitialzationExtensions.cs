using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace CommunityToolkit.Mvvm.DependencyInjection.Microsoft
{
    public static class MicrosoftIocInitialzationExtensions
    {
        public static void BuilderService(Action<IServiceCollection> builder_service, Action<IServiceProvider> config_service)
        {
            var service = new ServiceCollection();
            service.TryAddSingleton<IServiceCollection>(service);
            service.TryAddSingleton<ISerivceProviderIsKeyedServiceType, ServiceInfo>();
            service.RegisterRequiredTypes(builder_service);
            var service_provider= service.BuildServiceProvider();
            ConfigService(service_provider, config_service);

        }
        
        private static void ConfigService(IServiceProvider provider, Action<IServiceProvider> config_service)
        {
            provider.ConfigPrismSerivce();
            config_service?.Invoke(provider);
        }

       

        private class ServiceInfo : ISerivceProviderIsKeyedServiceType
        {
            private IServiceCollection _services;
            private IServiceProviderIsKeyedService _provider;
            public ServiceInfo(IServiceCollection services,IServiceProviderIsKeyedService serviceProvider)
            {
                _services = services;
                _provider= serviceProvider;
            }
            public Type? GetRegistrationType(string key)
            {
                string key2 = key;
                var serviceRegistrationInfo = (from r in _services
                                               where r.IsKeyedService && key2.Equals(r.ServiceKey?.ToString(), StringComparison.Ordinal)
                                               select r).FirstOrDefault();
                if (serviceRegistrationInfo is null)
                {
                    serviceRegistrationInfo = (from r in _services
                                               where r.IsKeyedService && key2.Equals(r.KeyedImplementationType.Name, StringComparison.Ordinal)
                                               select r).FirstOrDefault();
                }
                return serviceRegistrationInfo?.KeyedImplementationType;

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
