using CommunityToolkit.Mvvm.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Modularity
{
    public class ModuleManager
    {
        private IReadOnlyCollection<DependencyEntry> DependencyEntries => DependencyEntry.AllDependencies;
        private IList<DependencyEntry> Sort { get; set; }
        public ModuleManager() {

        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        public void AddModule<TModule>() where TModule : class, IModule, new()
        {
    
            DependencyEntry.TryAddDependencyEntry(typeof(TModule), out var dependencyEntry);
        }

        internal void ConfigModuleService(IServiceCollection serviceDescriptors)
        {
            Sort = DAGSortHelper.DAGSort(DependencyEntries, p => p.Dependencys);
            foreach (var dep in Sort)
            {
                var obj = Activator.CreateInstance(dep.Module) as IModule;
                if (obj != null)
                {
                    obj.ConfigService(serviceDescriptors);
                    serviceDescriptors.AddSingleton(dep.Module, obj);
                }
            }
        }

        internal void OnInitialized(IServiceProvider serviceProvider)
        {
            if (Sort?.Count > 0)
            {
                foreach(var dep in Sort)
                {
                    var obj= serviceProvider.GetService(dep.Module) as IModule;
                    obj.OnInitialized(serviceProvider);
                }
            }
        }
    }
}
