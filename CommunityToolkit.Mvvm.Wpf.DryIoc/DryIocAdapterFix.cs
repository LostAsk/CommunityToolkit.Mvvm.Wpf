using DryIoc.Microsoft.DependencyInjection;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryIoc
{
    public static class DryIocAdapterFix
    {
        /// <summary>Adapts passed <paramref name="container"/> to Microsoft.DependencyInjection conventions,
        /// registers DryIoc implementations of <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>,
        /// and returns NEW container.
        /// </summary>
        /// <param name="container">Source container to adapt.</param>
        /// <param name="descriptors">(optional) Specify service descriptors or use <see cref="Populate"/> later.</param>
        /// <param name="registerDescriptor">(optional) Custom registration action, should return true to skip normal registration.</param>
        /// <param name="registrySharing">(optional) Use DryIoc <see cref="RegistrySharing"/> capability.</param>
        /// <example>
        /// <code><![CDATA[
        /// 
        ///     var container = new Container();
        ///
        ///     // you may register the services here:
        ///     container.Register<IMyService, MyService>(Reuse.Scoped)
        /// 
        ///     // applies the MS.DI rules and registers the infrastructure helpers and service collection to the container
        ///     var adaptedContainer = container.WithDependencyInjectionAdapter(services); 
        ///
        ///     // the container implements IServiceProvider
        ///     IServiceProvider serviceProvider = adaptedContainer;
        ///
        ///]]></code>
        /// </example>
        /// <remarks>You still need to Dispose adapted container at the end / application shutdown.</remarks>
        public static IContainer WithDependencyInjectionAdapterNew(this IContainer container,
            IEnumerable<ServiceDescriptor> descriptors = null,
            Func<IRegistrator, ServiceDescriptor, bool> registerDescriptor = null,
            RegistrySharing registrySharing = RegistrySharing.Share)
        {
            var hasMicrosoftDependencyInjectionRules = container.Rules.HasMicrosoftDependencyInjectionRules();
            if (!hasMicrosoftDependencyInjectionRules)
                container = container.With(container.Rules.WithMicrosoftDependencyInjectionRules(), container.ScopeContext, registrySharing, container.SingletonScope);
            else if (registrySharing != RegistrySharing.Share)
                container = container.With(container.Rules, container.ScopeContext, registrySharing, container.SingletonScope);

            var capabilities = new DryIocServiceProviderCapabilities(container);
            var singletons = container.SingletonScope;
            singletons.Use<IServiceProviderIsService>(capabilities);
            singletons.Use<ISupportRequiredService>(capabilities);
            singletons.Use<IServiceScopeFactory>(capabilities);

            if (descriptors != null)
                container.Populate(descriptors, registerDescriptor);

            return container;
        }


        public static void Populate(this IContainer container, IEnumerable<ServiceDescriptor> descriptors,
            Func<IRegistrator, ServiceDescriptor, bool> registerDescriptor = null)
        {
            if (registerDescriptor == null)
                foreach (var descriptor in descriptors)
                    container.RegisterDescriptor(descriptor);
            else
                foreach (var descriptor in descriptors)
                    if (!registerDescriptor(container, descriptor))
                        container.RegisterDescriptor(descriptor);
        }


        public static void RegisterDescriptor(this IContainer container, ServiceDescriptor descriptor) =>
            container.RegisterDescriptorFix(descriptor, IfAlreadyRegistered.AppendNotKeyed);

        /// <summary>Unpacks the service descriptor to register the service in DryIoc container
        /// with the specific `IfAlreadyRegistered` policy and the optional `serviceKey`</summary>
        public static void RegisterDescriptorFix(this IContainer container, ServiceDescriptor descriptor, IfAlreadyRegistered ifAlreadyRegistered,
            object serviceKey = null)
        {
            var serviceType = descriptor.ServiceType;
            var implType = descriptor.IsKeyedService ? descriptor.KeyedImplementationType : descriptor.ImplementationType;
            //add this code
            serviceKey = serviceKey ?? descriptor.ServiceKey;
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
