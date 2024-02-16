using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunityToolkit.Mvvm.DependencyInjection.DryIoc
{
    public static class DryIocInitialzationExtensions
    {
        public static void PopulateKey(this IContainer container, IEnumerable<ServiceDescriptor> descriptors,
            Func<IRegistrator, ServiceDescriptor, bool> registerDescriptor = null)
        {
            if (registerDescriptor == null)
                foreach (var descriptor in descriptors)
                    container.RegisterDescriptorNew(descriptor);
            else
                foreach (var descriptor in descriptors)
                    if (!registerDescriptor(container, descriptor))
                        container.RegisterDescriptorNew(descriptor);



        }

        public static void RegisterDescriptorNew(this IContainer container, ServiceDescriptor descriptor, IfAlreadyRegistered? ifAlreadyRegistered=null)
        {
            var serviceType = descriptor.ServiceType;
            var implType = descriptor.IsKeyedService ? descriptor.KeyedImplementationType : descriptor.ImplementationType;
            var serviceKey = descriptor.ServiceKey;
            if (implType != null)
            {
                container.Register(ReflectionFactory.Of(implType, descriptor.Lifetime.ToReuse()), serviceType,
                    serviceKey, ifAlreadyRegistered, isStaticallyChecked: implType == serviceType);
            }
            else if (descriptor.ImplementationFactory != null)
            {
                container.Register(DelegateFactory.Of(descriptor.ImplementationFactory.ToFactoryDelegate, descriptor.Lifetime.ToReuse()), serviceType,
                    serviceKey, ifAlreadyRegistered, isStaticallyChecked: true);
            }
            else
            {
                var instance = descriptor.ImplementationInstance;
                container.Register(InstanceFactory.Of(instance), serviceType,
                    serviceKey, ifAlreadyRegistered, isStaticallyChecked: true);
                container.TrackDisposable(instance); // todo: @wip @incompatible calling this method depend on the `ifAlreadyRegistered` policy
            }
        }
        public static void BuilderService(Action<IContainer> builder_service, Action<IServiceProvider> config_service)
        {
            var service = new Container();

            service.RegisterInstance<IContainer>(service);
            service.Register<ServiceInfo>(Reuse.Singleton);
            service.RegisterDelegate<ISerivceProviderIsKeyedServiceType>((r) => r.Resolve<ServiceInfo>(), Reuse.Singleton);
            service.RegisterDelegate<IServiceProviderIsService>((r) => r.Resolve<ServiceInfo>(), Reuse.Singleton);
            service.RegisterDelegate<IServiceProviderIsKeyedService>((r) => r.Resolve<ServiceInfo>(), Reuse.Singleton);
            var servicez = new ServiceCollection();
            servicez.RegisterRequiredTypes(null);
            service.Populate(servicez);
            builder_service?.Invoke(service);
            ConfigService(service, config_service);
        }

        private static void ConfigService(IServiceProvider provider, Action<IServiceProvider> config_service)
        {
            provider.ConfigPrismSerivce();
            config_service?.Invoke(provider);
        }



        private class ServiceInfo : ISerivceProviderIsKeyedServiceType
        {
            private IContainer Instance;
            public ServiceInfo(IContainer services)
            {
                Instance = services;
            }
            public Type GetRegistrationType(string key)
            {
                string key2 = key;
                ServiceRegistrationInfo serviceRegistrationInfo = (from r in Instance.GetServiceRegistrations()
                                                                   where key2.Equals(r.OptionalServiceKey?.ToString(), StringComparison.Ordinal)
                                                                   select r).FirstOrDefault();
                if (serviceRegistrationInfo.OptionalServiceKey == null)
                {
                    serviceRegistrationInfo = (from r in Instance.GetServiceRegistrations()
                                               where key2.Equals(r.ImplementationType?.Name, StringComparison.Ordinal)
                                               select r).FirstOrDefault();
                }

                return serviceRegistrationInfo.ImplementationType;

            }

            public bool IsKeyedService(Type serviceType, object serviceKey)
            {
                if (!Instance.IsRegistered(serviceType, serviceKey))
                {
                    return Instance.IsRegistered(serviceType, serviceKey, FactoryType.Wrapper);
                }

                return true;
            }

            public bool IsService(Type serviceType)
            {
                return Instance.IsRegistered(serviceType);
            }
        }



    }
}
