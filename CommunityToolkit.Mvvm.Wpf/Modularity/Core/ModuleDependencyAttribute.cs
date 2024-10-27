using System;

namespace CommunityToolkit.Mvvm.Modularity
{
    /// <summary>
    /// 模块依赖 <see cref="IModule"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ModuleDependencyAttribute : Attribute
    {

        /// <summary>
        /// 初始化 <see cref="ModuleDependencyAttribute"/>.
        /// </summary>
        /// <param name="moduleName">依赖的实现IModule模块类型</param>
        public ModuleDependencyAttribute(Type moduleName)
        {
            ModuleType = moduleName;
        }

        /// <summary>
        /// 获取依赖项
        /// </summary>
        /// <value></value>
        public Type ModuleType { get; }
    }
}
