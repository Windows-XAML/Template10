using Prism.Navigation;
using System;
using Windows.Foundation;
using Windows.UI.Core;

namespace Template10.Navigation
{
    public static class Extensions
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
            return (parameters as INavigationParametersInternal).GetValue<NavigationMode>(nameof(NavigationMode));
        }
        public static NavigationService GetNavigationService(this INavigationParameters parameters)
        {
            return (parameters as INavigationParametersInternal).GetValue<NavigationService>(nameof(NavigationService));
        }
        public static CoreDispatcher GetDispatcher(this INavigationParameters parameters)
        {
            return (parameters as INavigationParametersInternal).GetValue<CoreDispatcher>(nameof(CoreDispatcher));
        }
        public static bool TryGetParameter<T>(this Windows.UI.Xaml.Navigation.NavigationEventArgs args, string name, out T value)
        {
            try
            {
                var www = new WwwFormUrlDecoder(args.Parameter.ToString());
                var result = www.GetFirstValueByName(name);
                value = (T)Convert.ChangeType(result, typeof(T));
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }
    }
}
