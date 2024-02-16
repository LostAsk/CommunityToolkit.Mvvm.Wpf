using CommunityToolkit.Mvvm.Helper;
using System;
using System.Collections.Specialized;
using System.Linq;

using System.Windows;


namespace CommunityToolkit.Mvvm.Navigation.Regions.Behaviors
{
    /// <summary>
    /// Behavior that monitors a <see cref="IRegion"/> object and
    /// changes the value for the <see cref="IActiveAware.IsActive"/> property when
    /// an object that implements <see cref="IActiveAware"/> gets added or removed
    /// from the collection.
    /// </summary>
    /// <remarks>
    /// This class can also sync the active state for any scoped regions directly on the view based on the <see cref="SyncActiveStateAttribute"/>.
    /// If you use the <see cref="Region.Add(object,string,bool)" /> method with the createRegionManagerScope option, the scoped manager will be attached to the view.
    /// </remarks>
    public class RegionActiveAwareBehavior : IRegionBehavior
    {
        /// <summary>
        /// Name that identifies the <see cref="RegionActiveAwareBehavior"/> behavior in a collection of <see cref="IRegionBehavior"/>.
        /// </summary>
        public const string BehaviorKey = "ActiveAware";

        /// <summary>
        /// The region that this behavior is extending
        /// </summary>
        public IRegion Region { get; set; }

        /// <summary>
        /// Attaches the behavior to the specified region
        /// </summary>
        public void Attach()
        {
            //获取Region的active views集合 进行事件注册
            INotifyCollectionChanged collection = this.GetCollection();
            if (collection != null)
            {
                //
                collection.CollectionChanged += OnCollectionChanged;
            }
        }

        /// <summary>
        /// Detaches the behavior from the <see cref="INotifyCollectionChanged"/>.
        /// </summary>
        public void Detach()
        {
            INotifyCollectionChanged collection = this.GetCollection();
            if (collection != null)
            {
                collection.CollectionChanged -= OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
          
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    Action<IActiveAware> invocation = activeAware => activeAware.IsActive = true;
                    //就是看看 新增的obj,是不是IActiveAware对象是的话 设置IsActive =true
                    MvvmHelpers.ViewAndViewModelAction(item, invocation);
                    //如果item为 依赖对象，看看它所属的regionmanger 是不是当前region的regionmanger一样
                    //不一样的话,看看这个对象的view类/vm类 有没SyncActiveStateAttribute标注
                    //有的话就把这个IsActive也标注为ture

                    //由前面所知 IRegion的add会根据需求创建新的 regionManger,这样当这个item变更时
                    //会通知对应的regionManger其他的regions更新咯
                    InvokeOnSynchronizedActiveAwareChildren(item, invocation);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    Action<IActiveAware> invocation = activeAware => activeAware.IsActive = false;

                    MvvmHelpers.ViewAndViewModelAction(item, invocation);
                    InvokeOnSynchronizedActiveAwareChildren(item, invocation);
                }
            }

            // May need to handle other action values (reset, replace). Currently the ViewsCollection class does not raise CollectionChanged with these values.
        }

        private void InvokeOnSynchronizedActiveAwareChildren(object item, Action<IActiveAware> invocation)
        {
            var dependencyObjectView = item as DependencyObject;

            if (dependencyObjectView != null)
            {
                // We are assuming that any scoped region managers are attached directly to the
                // view.
                var regionManager = RegionManager.GetRegionManager(dependencyObjectView);

                // If the view's RegionManager attached property is different from the region's RegionManager,
                // then the view's region manager is a scoped region manager.
                if (regionManager == null || regionManager == this.Region.RegionManager) return;

                var activeViews = regionManager.Regions.SelectMany(e => e.ActiveViews);

                var syncActiveViews = activeViews.Where(ShouldSyncActiveState);

                foreach (var syncActiveView in syncActiveViews)
                {
                    MvvmHelpers.ViewAndViewModelAction(syncActiveView, invocation);
                }
            }
        }

        private bool ShouldSyncActiveState(object view)
        {
            if (Attribute.IsDefined(view.GetType(), typeof(SyncActiveStateAttribute)))
            {
                return true;
            }

            var viewAsFrameworkElement = view as FrameworkElement;

            if (viewAsFrameworkElement != null)
            {
                var viewModel = viewAsFrameworkElement.DataContext;

                return viewModel != null && Attribute.IsDefined(viewModel.GetType(), typeof(SyncActiveStateAttribute));
            }

            return false;
        }

        private INotifyCollectionChanged GetCollection()
        {
            return this.Region.ActiveViews;
        }
    }
}
