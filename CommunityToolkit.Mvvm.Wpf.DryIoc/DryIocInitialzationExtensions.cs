using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Example;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CommunityToolkit.Mvvm.DependencyInjection.DryIoc
{
    public abstract class DryIocInitialzation : Initialization
    {
        protected IContainer Container { get;private set; }


        protected virtual IContainer CreateContainer()
        {
            return  new Container();
        }
        protected override IServiceProvider RegisterTypesAndBuilderIServiceProvider(IServiceCollection serviceDescriptors)
        {
            Container = CreateContainer();
            Container.RegisterInstance<IContainer>(Container);
            Container.Register<ServiceInfo>(Reuse.Singleton);
            Container.RegisterDelegate<ISerivceProviderIsKeyedServiceType>((r) => r.Resolve<ServiceInfo>(), Reuse.Singleton);
            Container.RegisterDelegate<IServiceProviderIsService>((r) => r.Resolve<ServiceInfo>(), Reuse.Singleton);
            Container.RegisterDelegate<IServiceProviderIsKeyedService>((r) => r.Resolve<ServiceInfo>(), Reuse.Singleton);
            Container.PopulateKey(serviceDescriptors);
            RegisterTypes(Container);
            return Container.BuildServiceProvider();
        }
        protected abstract void RegisterTypes(IContainer serviceDescriptors);

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



    public static class DryIocExtensions
    {

 

        public static void BuilderViewAndViewModelByDryIoc(this IContainer container,params Assembly[] assemblies)
        {
            container.PopulateKey(assemblies.SelectMany(x => x.RegisterViewAndViewModel()), (reg, ser) =>
            {
                return ser.IsKeyedService ? reg.IsRegistered(ser.ServiceType, ser.ServiceKey) : reg.IsRegistered(ser.ServiceType);
            });
        }


        public static void PopulateKey(this IContainer container, IEnumerable<ServiceDescriptor> descriptors,
    Func<IRegistrator, ServiceDescriptor, bool> registerDescriptor = null)
        {
            if (registerDescriptor == null)
                foreach (var descriptor in descriptors)
                    container.RegisterDescriptorNew(descriptor, IfAlreadyRegistered.AppendNotKeyed);
            else
                foreach (var descriptor in descriptors)
                    if (!registerDescriptor(container, descriptor))
                        container.RegisterDescriptorNew(descriptor, IfAlreadyRegistered.AppendNotKeyed);



        }

        public static void RegisterDescriptorNew(this IContainer container, ServiceDescriptor descriptor, IfAlreadyRegistered? ifAlreadyRegistered = null)
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
       



    }
}
