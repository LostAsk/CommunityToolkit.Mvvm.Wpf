#if HAS_WINUI
using Microsoft.UI.Xaml;
#else
using System.Windows;
#endif

namespace CommunityToolkit.Mvvm.Navigation.Regions.Behaviors
{
    /// <summary>
    /// Defines a <see cref="IRegionBehavior"/> that not allows extensible behaviors on regions which also interact
    /// with the target element that the <see cref="IRegion"/> is attached to.
    /// </summary>
    public interface IHostAwareRegionBehavior : IRegionBehavior
    {
        /// <summary>
        /// 区域的所在控件
        /// Gets or sets the <see cref="DependencyObject"/> that the <see cref="IRegion"/> is attached to.
        /// </summary>
        /// <value>A <see cref="DependencyObject"/> that the <see cref="IRegion"/> is attached to.
        /// This is usually a <see cref="FrameworkElement"/> that is part of the tree.</value>
        DependencyObject HostControl { get; set; }
    }
}
