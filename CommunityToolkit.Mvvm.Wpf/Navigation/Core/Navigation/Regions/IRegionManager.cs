using System;

namespace CommunityToolkit.Mvvm.Navigation.Regions
{
    /// <summary>
    /// Defines an interface to manage a set of <see cref="IRegion">regions</see> and to attach regions to objects (typically controls).
    /// </summary>
    public interface IRegionManager
    {
        /// <summary>
        /// Gets a collection of <see cref="IRegion"/> that identify each region by name. You can use this collection to add or remove regions to the current region manager.
        /// </summary>
        IRegionCollection Regions { get; }

        /// <summary>
        /// Creates a new region manager.
        /// </summary>
        /// <returns>A new region manager that can be used as a different scope from the current region manager.</returns>
        IRegionManager CreateRegionManager();

        /// <summary>
        /// Add a view to the Views collection of a Region. Note that the region must already exist in this <see cref="IRegionManager"/>.
        /// </summary>
        /// <param name="regionName">The name of the region to add a view to</param>
        /// <param name="view">The view to add to the views collection</param>
        /// <returns>The RegionManager, to easily add several views. </returns>
        IRegionManager AddToRegion(string regionName, object view);

        /// <summary>
        /// Add a view to the Views collection of a Region. Note that the region must already exist in this <see cref="IRegionManager"/>.
        /// </summary>
        /// <param name="regionName">The name of the region to add a view to</param>
        /// <param name="viewName">The view to add to the views collection</param>
        /// <returns>The RegionManager, to easily add several views. </returns>
        IRegionManager AddToRegion(string regionName, string viewName);

        /// <summary>
        /// This method allows an IRegionManager to locate a specified region and navigate in it to the specified target Uri, passing a navigation callback and an instance of NavigationParameters, which holds a collection of object parameters.
        /// </summary>
        /// <param name="regionName">The name of the region where the navigation will occur.</param>
        /// <param name="target">A Uri that represents the target where the region will navigate.</param>
        /// <param name="navigationCallback">The navigation callback that will be executed after the navigation is completed.</param>
        /// <param name="navigationParameters">An instance of NavigationParameters, which holds a collection of object parameters.</param>
        void RequestNavigate(string regionName, Uri target, Action<NavigationResult> navigationCallback, INavigationParameters navigationParameters);
    }
}
