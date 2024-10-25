using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public static class IServiceCollectionRegistryExtensions
    {
        //
        // 摘要:
        //     Registers an object to be used as a dialog in the IDialogService.
        //
        // 参数:
        //   containerRegistry:
        //
        //   name:
        //     The unique name to register with the dialog.
        //
        // 类型参数:
        //   TView:
        //     The Type of object to register as the dialog
        public static void RegisterDialog<TView>(this IServiceCollection containerRegistry, string name = null)
        {
            containerRegistry.RegisterForNavigation<TView>(name);
        }

        //
        // 摘要:
        //     Registers an object to be used as a dialog in the IDialogService.
        //
        // 参数:
        //   containerRegistry:
        //
        //   name:
        //     The unique name to register with the dialog.
        //
        // 类型参数:
        //   TView:
        //     The Type of object to register as the dialog
        //
        //   TViewModel:
        //     The ViewModel to use as the DataContext for the dialog
        //public static void RegisterDialog<TView, TViewModel>(this IServiceCollection containerRegistry, string name = null) where TViewModel : IDialogAware
        //{
        //    containerRegistry.RegisterForNavigation<TView, TViewModel>(name);
        //}

        //
        // 摘要:
        //     Registers an object that implements IDialogWindow to be used to host all dialogs
        //     in the IDialogService.
        //
        // 参数:
        //   containerRegistry:
        //
        // 类型参数:
        //   TWindow:
        //     The Type of the Window class that will be used to host dialogs in the IDialogService
        //public static void RegisterDialogWindow<TWindow>(this IContainerRegistry containerRegistry) where TWindow : IDialogWindow
        //{
        //    containerRegistry.Register(typeof(IDialogWindow), typeof(TWindow));
        //}

        //
        // 摘要:
        //     Registers an object for navigation
        //
        // 参数:
        //   containerRegistry:
        //
        //   type:
        //     The type of object to register
        //
        //   name:
        //     The unique name to register with the obect.
        public static void RegisterForNavigation(this IServiceCollection containerRegistry, Type type, string name)
        {
            containerRegistry.AddKeyedTransient(typeof(object), name, type);
        }

        //
        // 摘要:
        //     Registers an object for navigation.
        //
        // 参数:
        //   containerRegistry:
        //
        //   name:
        //     The unique name to register with the object.
        //
        // 类型参数:
        //   T:
        //     The Type of the object to register as the view
        public static void RegisterForNavigation<T>(this IServiceCollection containerRegistry, string name = null)
        {
            Type typeFromHandle = typeof(T);
            string name2 = (string.IsNullOrWhiteSpace(name) ? typeFromHandle.Name : name);
            containerRegistry.RegisterForNavigation(typeFromHandle, name2);
        }

        //
        // 摘要:
        //     Registers an object for navigation with the ViewModel type to be used as the
        //     DataContext.
        //
        // 参数:
        //   containerRegistry:
        //
        //   name:
        //     The unique name to register with the view
        //
        // 类型参数:
        //   TView:
        //     The Type of object to register as the view
        //
        //   TViewModel:
        //     The ViewModel to use as the DataContext for the view
        public static void RegisterForNavigation<TView, TViewModel>(this IServiceCollection containerRegistry, string name = null)
        {
            containerRegistry.RegisterForNavigationWithViewModel<TViewModel>(typeof(TView), name);
        }

        private static void RegisterForNavigationWithViewModel<TViewModel>(this IServiceCollection containerRegistry, Type viewType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = viewType.Name;
            }

            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));
            containerRegistry.RegisterForNavigation(viewType, name);
        }
    }
}
