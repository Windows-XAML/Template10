using Prism.Navigation;
using Windows.UI.Core;

namespace Prism.Windows.Navigation
{
    public static partial class Extensions
    {
        internal static void SetNavigationMode(this INavigationParameters parameters, NavigationMode mode)
        {
            (parameters as INavigationParametersInternal).Add(nameof(NavigationMode), mode);
        }
        internal static void SetNavigationService(this INavigationParameters parameters, IPlatformNavigationService service)
        {
            (parameters as INavigationParametersInternal).Add(nameof(NavigationService), service);
        }
        internal static void SetDispatcher(this INavigationParameters parameters, CoreDispatcher dispatcher)
        {
            (parameters as INavigationParametersInternal).Add(nameof(CoreDispatcher), dispatcher);
        }
        public static NavigationMode GetNavigationMode(this INavigationParameters parameters)
        {
            if (!(parameters as INavigationParametersInternal).ContainsKey(nameof(NavigationMode))) return default(NavigationMode);
            return (parameters as INavigationParametersInternal).GetValue<NavigationMode>(nameof(NavigationMode));
        }
        public static NavigationService GetNavigationService(this INavigationParameters parameters)
        {
            if (!(parameters as INavigationParametersInternal).ContainsKey(nameof(NavigationService))) return null;
            return (parameters as INavigationParametersInternal).GetValue<NavigationService>(nameof(NavigationService));
        }
        public static CoreDispatcher GetDispatcher(this INavigationParameters parameters)
        {
            if (!(parameters as INavigationParametersInternal).ContainsKey(nameof(CoreDispatcher))) return null;
            return (parameters as INavigationParametersInternal).GetValue<CoreDispatcher>(nameof(CoreDispatcher));
        }
    }

}
