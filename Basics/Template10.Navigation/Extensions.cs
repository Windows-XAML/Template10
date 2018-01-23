using Prism.Navigation;

namespace Template10.Navigation
{
    public static class Extensions
    {
        internal static void SetNavigationMode(this INavigationParameters parameters, NavigationMode mode)
        {
            (parameters as INavigationParametersInteral).AddInternalParameter(nameof(NavigationMode), mode);
        }
        internal static void SetNavigationService(this INavigationParameters parameters, INavigationServiceUwp service)
        {
            (parameters as INavigationParametersInteral).AddInternalParameter(nameof(NavigationService), service);
        }
        public static NavigationMode GetNavigationMode(this INavigationParameters parameters)
        {
            return (parameters as INavigationParametersInteral).GetValue<NavigationMode>(nameof(NavigationMode));
        }
        public static NavigationService GetNavigationService(this INavigationParameters parameters)
        {
            return (parameters as INavigationParametersInteral).GetValue<NavigationService>(nameof(NavigationService));
        }
    }
}
