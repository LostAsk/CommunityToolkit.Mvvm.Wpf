using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;

namespace CommunityToolkit.Mvvm.Modularity
{
    /// <summary>
    /// 模块接口
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        void ConfigService(IServiceCollection services);

        /// <summary>
        /// 初始化
        /// </summary>
        void OnInitialized(IServiceProvider serviceProvider);
    }
}
