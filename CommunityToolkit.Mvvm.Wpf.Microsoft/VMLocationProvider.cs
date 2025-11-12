using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityToolkit.Mvvm.Wpf.Microsoft
{
    /// <summary>
    /// 视图定位器
    /// </summary>
    public class VMLocationProvider
    {
        public static VMLocationProvider Instance { get; } = new();
        private VMLocationProvider() { }

        private IServiceProvider _serviceProvider { get; set; }

        Func<object, IServiceProvider, object> _defaultViewModelFactory;

        Func<string, IServiceProvider, object> _defaultViewFactory;

        Dictionary<string, Type> Mapping { get; } = [];

        public VMLocationProvider SetServiceProvider(IServiceProvider serviceProvider)
        {
            if(_serviceProvider is not null)
            {
                throw new ArgumentNullException("服务已经设置过了");
            }
            _serviceProvider = serviceProvider;
            return this;
        }

        /// <summary>
        ///  设置View对应的Vm自定义工厂
        /// </summary>
        /// <param name="viewModelFactory">(视图类型)=>vm</param>
        public VMLocationProvider SetDefaultViewModelFactory(Func<object, IServiceProvider, object> viewModelFactory)
        {
            _defaultViewModelFactory = viewModelFactory;
            return this;
        }

        /// <summary>
        /// 根据key 创建对应视图
        /// </summary>
        /// <param name="viewFactory"></param>
        /// <returns></returns>
        public VMLocationProvider SetDefaultViewFactory(Func<string, IServiceProvider, object> viewFactory)
        {
            _defaultViewFactory = viewFactory;
            return this;
        }

        public VMLocationProvider AddTypeMapping(string key,Type type)
        {
            Mapping[key] = type;
            return this;
        }

        /// <summary>
        /// 获取视图类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Type? ResolveViewType(string key)
        {
            if(Mapping.TryGetValue(key,out var t))
            {
                return t; 
            }
            return null;
        }

        /// <summary>
        /// 创建视图
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object ResolveView(string key)
        {
            return _defaultViewFactory(key, _serviceProvider);
        }

        /// <summary>
        /// 对视图自动注入vm
        /// </summary>
        /// <param name="view"></param>
        /// <param name="setDataContextCallback"></param>
        public void AutoWireViewModelChanged(object view, Action<object, object> setDataContextCallback)
        {
            var vm=_defaultViewModelFactory(view, _serviceProvider);
            setDataContextCallback(view, vm);
        }
    }

     

}
