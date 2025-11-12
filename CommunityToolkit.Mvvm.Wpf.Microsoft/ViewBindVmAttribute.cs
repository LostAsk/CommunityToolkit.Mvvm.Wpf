using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 绑定 View-ViewModel关系
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ViewBindVmAttribute: Attribute
    {
        /// <summary>
        /// 将当前实现类型注册为指定服务的特性
        /// </summary>
        /// <param name="lifetime">Vm生命周期</param>
        /// <param name="viewType">注册的View类型</param>
        /// <param name="build_view_instance_key">创建view实例的key,方便后续showdialog</param>
        public ViewBindVmAttribute(ServiceLifetime lifetime,Type viewType,string build_view_instance_key=null)
        {
        }
    }
}
