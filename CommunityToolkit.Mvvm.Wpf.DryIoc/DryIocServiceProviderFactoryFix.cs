using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryIoc
{
    public class DryIocServiceProviderFactoryFix : IServiceProviderFactory<IContainer>
    {
        private readonly IContainer _container;
        private readonly Func<IRegistrator, ServiceDescriptor, bool> _registerDescriptor;
        private readonly RegistrySharing _registrySharing;

        /// <summary>
        /// We won't initialize the container here because it is logically expected to be done in `CreateBuilder`,
        /// so the factory constructor is just saving some options to use later.
        /// </summary>
        public DryIocServiceProviderFactoryFix(
            IContainer container = null,
            Func<IRegistrator, ServiceDescriptor, bool> registerDescriptor = null) :
            this(container, RegistrySharing.CloneAndDropCache, registerDescriptor)
        { }

        /// <summary>
        /// `container` is the existing container which will be cloned with the MS.DI rules and its cache will be dropped,
        /// unless the `registrySharing` is set to the `RegistrySharing.Share` or to `RegistrySharing.CloneButKeepCache`.
        /// `registerDescriptor` is the custom service descriptor handler.
        /// </summary>
        public DryIocServiceProviderFactoryFix(
            IContainer container,
            RegistrySharing registrySharing,
            Func<IRegistrator, ServiceDescriptor, bool> registerDescriptor = null)
        {
            _container = container;
            _registrySharing = registrySharing;
            _registerDescriptor = registerDescriptor;
        }

        /// <inheritdoc />
        public IContainer CreateBuilder(IServiceCollection services) =>
            (_container ?? new Container(Rules.MicrosoftDependencyInjectionRules))
                .WithDependencyInjectionAdapterNew(services, _registerDescriptor, _registrySharing);

        /// <summary>The <paramref name="container"/> is the container returned by the <see cref="CreateBuilder(IServiceCollection)"/> method.</summary>
        public IServiceProvider CreateServiceProvider(IContainer container) =>
            container.BuildServiceProvider();
    }
}
