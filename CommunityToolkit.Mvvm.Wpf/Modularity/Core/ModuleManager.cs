using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunityToolkit.Mvvm.Modularity
{
    public class ModuleManager
    {
        private IReadOnlyCollection<DependencyEntry> DependencyEntries => DependencyEntry.AllDependencies;
        private IList<DependencyEntry> Sort { get; set; }

        public Func<IServiceCollection, Type, IModule> BuilderModule { get; set; } = (s, t) => Activator.CreateInstance(t) as IModule;

        public Action<Type[]> RegisterTypes { get; set; }
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
            RegisterTypes?.Invoke(Sort.Select(p => p.Module).ToArray());
            foreach (var dep in Sort)
            {
                var obj = BuilderModule(serviceDescriptors,dep.Module);
                if (obj != null)
                {
                    obj.ConfigService(serviceDescriptors);
                    serviceDescriptors?.AddSingleton(dep.Module, obj);
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
