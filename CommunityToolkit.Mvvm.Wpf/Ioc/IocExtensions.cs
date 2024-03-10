using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Globalization;
using System.Linq;
namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public static class IocExtensions
    {
        public static IEnumerable<ServiceDescriptor> RegisterViewAndViewModel(this Assembly assembly)
        {
            foreach(var viewtype in assembly.GetTypes())
            {
                if (viewtype.FullName.Contains(".Views.")&&viewtype.IsClass&&!viewtype.IsAbstract&&!viewtype.IsGenericType)
                {
                  
                    yield return new ServiceDescriptor(viewtype, viewtype, ServiceLifetime.Transient);
                    yield return new ServiceDescriptor(viewtype,  viewtype.Name, viewtype, ServiceLifetime.Transient);
                    yield return new ServiceDescriptor(viewtype, viewtype.FullName, viewtype, ServiceLifetime.Transient);
                    var viewvmtype = DefaultViewTypeToViewModel(viewtype);
                    if(viewvmtype is not null)
                    {
                        ViewModelLocationProvider.Register(viewtype.Name, viewvmtype);
                        ViewModelLocationProvider.Register(viewtype.FullName, viewvmtype);
                        
                        yield return new ServiceDescriptor(viewvmtype, implementationType: viewvmtype, ServiceLifetime.Transient);
                    }
                
                }

            }




            static Type DefaultViewTypeToViewModel(Type viewType)
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
                return Type.GetType(viewModelName);
            }
        }

        public static Type IsRegistrationType(this IServiceProvider @ioc, string name)
        {
            
            return @ioc!.GetService<ISerivceProviderIsKeyedServiceType>().GetRegistrationType(name);
        }

        public static bool IsRegistrationType(this IServiceProvider @ioc, Type type)
        {
           
            return @ioc!.GetRequiredService<ISerivceProviderIsKeyedServiceType>().IsService(type);
        }

        public static bool IsRegistrationType(this IServiceProvider @ioc, Type type,object service_key)
        {
 
            return @ioc!.GetRequiredService<ISerivceProviderIsKeyedServiceType>().IsKeyedService(type, service_key);
        }
    }
}
