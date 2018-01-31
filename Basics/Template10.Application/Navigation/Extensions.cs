using Prism.Navigation;
using Windows.Foundation;

namespace Template10.Navigation
{
    public static class Extensions
    {
        internal static void SetNavigationMode(this INavigationParameters parameters, NavigationMode mode)
        {
            (parameters as INavigationParametersInternal).Add(nameof(NavigationMode), mode);
        }
        internal static void SetNavigationService(this INavigationParameters parameters, INavigationServiceUwp service)
        {
            (parameters as INavigationParametersInternal).Add(nameof(NavigationService), service);
        }
        public static NavigationMode GetNavigationMode(this INavigationParameters parameters)
        {
            return (parameters as INavigationParametersInternal).GetValue<NavigationMode>(nameof(NavigationMode));
        }
        public static NavigationService GetNavigationService(this INavigationParameters parameters)
        {
            return (parameters as INavigationParametersInternal).GetValue<NavigationService>(nameof(NavigationService));
        }
        public static NavigationParameters ToNavigationParameters(this WwwFormUrlDecoder query)
        {
            var p = new NavigationParameters();
            foreach (var item in query)
            {
                p.Add(item.Name, item.Value);
            }
            return p;
        }
    }
}
