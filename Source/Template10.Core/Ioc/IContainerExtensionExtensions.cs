using System;
using Template10.Navigation;
using Windows.UI.Xaml.Controls;

namespace Template10.Ioc
{
    public static partial class IContainerExtensionExtensions
    {
        public static object ResolveViewModelForView(this Prism.Ioc.IContainerExtension extension, object view, Type viewModelType)
        {
            if (view is Page page)
            {
                var service = NavigationService.Instances[page.Frame];
                return extension.Resolve(viewModelType, (typeof(INavigationService), service));
            }
            else
            {
                return extension.Resolve(viewModelType);
            }
        }
    }
}
