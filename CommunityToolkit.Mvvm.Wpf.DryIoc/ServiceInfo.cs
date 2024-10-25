using CommunityToolkit.Mvvm.DependencyInjection;
using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public class ServiceInfo : ISerivceProviderIsKeyedServiceType
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
