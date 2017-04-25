using System.Linq;
using Template10.Common;
using Template10.Services.NavigationService;
using Template10.Services.WindowWrapper;
using Windows.UI.Core;

namespace Template10.Utils
{
    public static class Template10Extensions
    {
        public static IWindowWrapper GetWindowWrapper(this INavigationService service)
        {
            return service.Window;
        }

        public static IDispatcherWrapper GetDispatcherWrapper(this INavigationService service)
        {
            return service.GetWindowWrapper().Dispatcher;
        }
    }
}
