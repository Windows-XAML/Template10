using System;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Utils
{
    public static class Template10Utils
    {
        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationService.GetForFrame(frame);

        public static bool Navigate(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => frame.GetNavigationService().Navigate(page, parameter, infoOverride);

        public static bool Navigate<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
            => frame.GetNavigationService().Navigate(key, parameter, infoOverride);
    }
}
