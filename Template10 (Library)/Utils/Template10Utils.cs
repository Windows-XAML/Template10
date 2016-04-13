using System;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Utils
{
    public static class Template10Utils
    {
        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationService.GetForFrame(frame);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => await frame.GetNavigationService().NavigateAsync(page, parameter, infoOverride);

        public static async Task<bool> NavigateAsyncEx<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
            => await frame.GetNavigationService().NavigateAsync(key, parameter, infoOverride);
    }
}
