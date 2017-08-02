using Template10.Common;
using Template10.Navigation;
using Template10.Core;

namespace Template10.Extensions
{
    public static class Template10Extensions
    {
        public static ITemplate10Window GetWindow(this INavigationService service)
        {
            return service.Window;
        }

        public static ITemplate10Dispatcher GetDispatcher(this INavigationService service)
        {
            return service.GetWindow().Dispatcher;
        }
    }
}
