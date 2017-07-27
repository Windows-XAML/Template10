using System;
using System.Threading.Tasks;
using Template10.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Extensions
{
    public static class NavigationExtensions
    {
        public static async Task<Frame> CreateNavigationService(this Frame frame, BackButton backButton = BackButton.Attach)
        {
            await NavigationService.CreateAsync(BackButton.Attach, frame);
            return frame;
        }

        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationService.Instances.GetByFrame(frame);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => await frame.GetNavigationService().NavigateAsync(page, parameter, infoOverride);

        public static async Task<bool> NavigateAsyncEx<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
            => await frame.GetNavigationService().NavigateAsync(key, parameter, infoOverride);
    }
}
