using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public static class INavigationServiceExtensions
    {
        public static Frame GetXamlFrame(this IPlatformNavigationService service)
        {
            return ((service as IPlatformNavigationService2).FrameFacade as IFrameFacade2).Frame;
        }

        [Obsolete]
        public static IPlatformNavigationService SetAsWindowContent(this IPlatformNavigationService service, Window window, bool activate)
        {
            window.Content = service.GetXamlFrame();
            if (activate)
            {
                window.Activate();
            }
            return service;
        }

        [Obsolete]
        public static async Task<INavigationResult> NavigateAsync(this IPlatformNavigationService service, string path, params (string Name, string Value)[] parameters)
        {
            return await service.NavigateAsync(PathBuilder.Create(path, parameters).ToString());
        }

        [Obsolete]
        public static async Task<INavigationResult> NavigateAsync(this IPlatformNavigationService service, string path, NavigationTransitionInfo infoOverride = null, params (string Name, string Value)[] parameters)
        {
            return await service.NavigateAsync(PathBuilder.Create(path, parameters).ToString(), null, infoOverride);
        }

        [Obsolete]
        public static Task RefreshAsync(this IPlatformNavigationService service)
            => (service as IPlatformNavigationService).RefreshAsync();

        [Obsolete]
        public static bool CanGoBack(this IPlatformNavigationService service)
            => (service as IPlatformNavigationService).CanGoBack();

        [Obsolete]
        public static Task GoBackAsync(this IPlatformNavigationService service, INavigationParameters parameters, NavigationTransitionInfo infoOverride)
            => (service as IPlatformNavigationService).RefreshAsync();

        [Obsolete]
        public static bool CanGoForward(this IPlatformNavigationService service)
            => (service as IPlatformNavigationService).CanGoForward();

        [Obsolete]
        public static Task GoForwardAsync(this IPlatformNavigationService service, INavigationParameters parameter)
            => (service as IPlatformNavigationService).GoForwardAsync(parameter);

        [Obsolete]
        public static Task<INavigationResult> NavigateAsync(this IPlatformNavigationService service, string path, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
            => (service as IPlatformNavigationService).NavigateAsync(path, parameter, infoOverride);

        [Obsolete]
        public static Task<INavigationResult> NavigateAsync(this IPlatformNavigationService service, Uri path, INavigationParameters parameter, NavigationTransitionInfo infoOverride)
            => (service as IPlatformNavigationService).NavigateAsync(path, parameter, infoOverride);
    }
}
