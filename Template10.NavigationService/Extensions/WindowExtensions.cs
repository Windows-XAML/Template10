using System.Linq;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.UI.Core;

namespace Template10.Utils
{
    public static class Template10Extensions
    {
        public static IWindowWrapper GetWindowWrapper(this INavigationService service)
            => Locator.WindowWrapper.Instances.FirstOrDefault(x => x.NavigationServices.Contains(service));

        public static IDispatcherWrapper GetDispatcherWrapper(this INavigationService service)
            => service.GetWindowWrapper().Dispatcher;
    }
}
