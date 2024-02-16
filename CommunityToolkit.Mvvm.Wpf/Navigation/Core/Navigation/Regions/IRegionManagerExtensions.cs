using System;
using CommunityToolkit.Mvvm.Navigation;

namespace CommunityToolkit.Mvvm.Navigation.Regions
{
    /// <summary>
    /// Common Extensions for the RegionManager
    /// </summary>
    public static class IRegionManagerExtensions
    {

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, Uri source) =>
            regionManager.RequestNavigate(regionName, source, _ => { }, new NavigationParameters());

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        /// <param name="navigationCallback">The navigation callback.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, Uri source, Action<NavigationResult> navigationCallback) =>
            regionManager.RequestNavigate(regionName, source, navigationCallback, new NavigationParameters());

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        /// <param name="navigationCallback">The navigation callback.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, string source, Action<NavigationResult> navigationCallback) =>
            regionManager.RequestNavigate(regionName, new Uri(source, UriKind.RelativeOrAbsolute), navigationCallback, new NavigationParameters());

        /// <summary>
        /// Navigates the specified region manager.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region to call Navigate on.</param>
        /// <param name="source">The URI of the content to display.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, string source) =>
            regionManager.RequestNavigate(regionName, new Uri(source, UriKind.RelativeOrAbsolute), _ => { }, new NavigationParameters());

        /// <summary>
        /// This method allows an IRegionManager to locate a specified region and navigate in it to the specified target string, passing a navigation callback and an instance of NavigationParameters, which holds a collection of object parameters.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region where the navigation will occur.</param>
        /// <param name="target">A string that represents the target where the region will navigate.</param>
        /// <param name="navigationCallback">The navigation callback that will be executed after the navigation is completed.</param>
        /// <param name="navigationParameters">An instance of NavigationParameters, which holds a collection of object parameters.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, string target, Action<NavigationResult> navigationCallback, INavigationParameters navigationParameters) =>
            regionManager.RequestNavigate(regionName, new Uri(target, UriKind.RelativeOrAbsolute), navigationCallback, navigationParameters);

        /// <summary>
        /// This method allows an IRegionManager to locate a specified region and navigate in it to the specified target Uri, passing an instance of NavigationParameters, which holds a collection of object parameters.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region where the navigation will occur.</param>
        /// <param name="target">A Uri that represents the target where the region will navigate.</param>
        /// <param name="navigationParameters">An instance of NavigationParameters, which holds a collection of object parameters.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, Uri target, INavigationParameters navigationParameters) =>
            regionManager.RequestNavigate(regionName, target, _ => { }, navigationParameters);

        /// <summary>
        /// This method allows an IRegionManager to locate a specified region and navigate in it to the specified target string, passing an instance of NavigationParameters, which holds a collection of object parameters.
        /// </summary>
        /// <param name="regionManager">The current <see cref="IRegionManager"/>.</param>
        /// <param name="regionName">The name of the region where the navigation will occur.</param>
        /// <param name="target">A string that represents the target where the region will navigate.</param>
        /// <param name="navigationParameters">An instance of NavigationParameters, which holds a collection of object parameters.</param>
        public static void RequestNavigate(this IRegionManager regionManager, string regionName, string target, INavigationParameters navigationParameters) =>
            regionManager.RequestNavigate(regionName, new Uri(target, UriKind.RelativeOrAbsolute), navigationParameters);
    }
}
