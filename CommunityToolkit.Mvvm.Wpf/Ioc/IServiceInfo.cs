using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public interface ISerivceProviderIsKeyedServiceType
        : IServiceProviderIsKeyedService
    {
        Type? GetRegistrationType(string key);
 
    }
}
