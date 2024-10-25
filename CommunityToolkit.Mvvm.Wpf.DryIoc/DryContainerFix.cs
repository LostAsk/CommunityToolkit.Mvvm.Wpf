using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryIoc
{
    public class DryKeyedContainer : Container, IKeyedServiceProvider
    {
        public DryKeyedContainer() : base() { }

        public DryKeyedContainer(Rules rules, IScopeContext scopeContext = null) : base(rules, scopeContext) { }

        public DryKeyedContainer(Func<Rules, Rules> configure, IScopeContext scopeContext = null) : base(configure, scopeContext) { }
        public object? GetKeyedService(Type serviceType, object? serviceKey)
        {
            return this.Resolve(serviceType, serviceKey);
        }

        public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
        {
            return this.Resolve(serviceType, serviceKey, IfUnresolved.Throw);
        }
    }
}
