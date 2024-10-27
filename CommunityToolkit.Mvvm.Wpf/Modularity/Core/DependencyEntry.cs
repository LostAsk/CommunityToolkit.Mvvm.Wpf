using CommunityToolkit.Mvvm.Modularity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Modularity
{
    internal class DependencyEntry
    {

        internal Type Module { get;  }

        internal IReadOnlyCollection<DependencyEntry> Dependencys { get; }

        private static Type Impc { get; }= typeof(IModule);

        internal static IReadOnlyCollection<DependencyEntry> AllDependencies => IModuleAllTypes;
        private static HashSet<DependencyEntry> IModuleAllTypes { get; } = new HashSet<DependencyEntry>(new DependencyEntryComp());
        private DependencyEntry(Type module) {
            if (!Impc.IsAssignableFrom(module))
            {
                throw new ArgumentException(@"传入参数必须 IModule 类型");
            }
            Module = module;
            Dependencys = FindModules(module);
            
        }
        public override string ToString()
        {
            return string.Join("->", YieldReturn(Module).Concat(Dependencys.Select(p=>p.Module)).Select(p=>p.Name));
        }

        private IEnumerable<T> YieldReturn<T>(T item)
        {
            yield return item;
        }

        private IReadOnlyCollection<DependencyEntry> FindModules(Type module)
        {
            var stack = new Stack<Type>();
            stack.Push(module);
            var list = new List<DependencyEntry>();
            while (stack.Count > 0)
            {
                var tmp= stack.Pop();

                var moduleDependencyAttributes = module.GetCustomAttributes(false).OfType<ModuleDependencyAttribute>()
                                            .Where(p => !p.ModuleType.IsAbstract
                                                    && Impc.IsAssignableFrom(p.ModuleType)
                                                    && !p.ModuleType.IsInterface
                                                    && p.ModuleType != tmp
                                            ).Select(p => p.ModuleType).ToArray();
                if (moduleDependencyAttributes.Length > 0)
                {
                    foreach(var tt in moduleDependencyAttributes)
                    {
                        if (list.Any(p=>p.Module==tt))
                        {
                            continue;
                        }
                        var m = IModuleAllTypes.FirstOrDefault(p => p.Module == tt);
                        if (m is null)
                        {
                            m = new DependencyEntry(tt);
                            stack.Push(tt);
                            IModuleAllTypes.Add(m);
                        }
                        list.Add(m);
                       
                    }
                }
            }
            list.Reverse();
            return list;
        }

        public static bool TryAddDependencyEntry(Type module,out DependencyEntry dependencyEntry)
        {
            dependencyEntry = default;
            if (IModuleAllTypes.Any(p => p.Module == module))
            {
                return false;
            }
            dependencyEntry =new DependencyEntry(module);
            IModuleAllTypes.Add(dependencyEntry);
            return true;
        }

        private class DependencyEntryComp : IEqualityComparer<DependencyEntry>
        {
            public bool Equals(DependencyEntry x, DependencyEntry y)
            {
                return x.Module.AssemblyQualifiedName == y.Module.AssemblyQualifiedName;
            }

            public int GetHashCode(DependencyEntry obj)
            {
                return obj.Module.AssemblyQualifiedName.GetHashCode();
            }
        }
    }
}
