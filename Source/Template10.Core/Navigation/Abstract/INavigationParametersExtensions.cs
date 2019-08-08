using System.Linq;
using System.Threading;

namespace Template10.Navigation
{
    public static class INavigationParametersExtensions
    {
        internal static void SetNavigationMode(this INavigationParameters parameters, NavigationMode mode)
        {
            if ((parameters as INavigationParametersInternal).ContainsKey(nameof(NavigationMode)))
            {
                (parameters as INavigationParametersInternal).Remove(nameof(NavigationMode));
            }
            (parameters as INavigationParametersInternal).Add(nameof(NavigationMode), mode);
        }

        internal static void SetNavigationService(this INavigationParameters parameters, INavigationService service)
        {
            if ((parameters as INavigationParametersInternal).ContainsKey(nameof(NavigationService)))
            {
                (parameters as INavigationParametersInternal).Remove(nameof(NavigationService));
            }
            (parameters as INavigationParametersInternal).Add(nameof(NavigationService), service);
        }

        internal static void SetSyncronizationContext(this INavigationParameters parameters, SynchronizationContext context)
        {
            if ((parameters as INavigationParametersInternal).ContainsKey(nameof(SynchronizationContext)))
            {
                (parameters as INavigationParametersInternal).Remove(nameof(SynchronizationContext));
            }
            (parameters as INavigationParametersInternal).Add(nameof(SynchronizationContext), context);
        }

        public static NavigationMode GetNavigationMode(this INavigationParameters parameters)
        {
            if (!(parameters as INavigationParametersInternal).ContainsKey(nameof(NavigationMode)))
            {
                return default(NavigationMode);
            }

            return (parameters as INavigationParametersInternal).GetValue<NavigationMode>(nameof(NavigationMode));
        }

        public static INavigationService GetNavigationService(this Windows.UI.Xaml.Controls.Frame frame)
        {
            return NavigationService.Instances.Single(x => x.Value.GetXamlFrame() == frame).Value;
        }

        public static INavigationService GetNavigationService(this INavigationParameters parameters)
        {
            if (!(parameters as INavigationParametersInternal).ContainsKey(nameof(NavigationService)))
            {
                return null;
            }

            return (parameters as INavigationParametersInternal).GetValue<INavigationService>(nameof(NavigationService));
        }

        public static SynchronizationContext GetSynchronizationContext(this INavigationParameters parameters)
        {
            if (!(parameters as INavigationParametersInternal).ContainsKey(nameof(SynchronizationContext)))
            {
                return null;
            }

            return (parameters as INavigationParametersInternal).GetValue<SynchronizationContext>(nameof(SynchronizationContext));
        }
    }
}
