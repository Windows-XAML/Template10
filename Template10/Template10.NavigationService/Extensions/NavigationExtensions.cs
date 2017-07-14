using System;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Utils
{
    public static class NavigationExtensions
    {
        public static async Task<Frame> RegisterAsync(this Frame frame, BackButton backButton = BackButton.Attach)
        {
            await NavigationServiceFactory.CreateAsync(BackButton.Attach, frame);
            return frame;
        }

        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationServiceHelper.Instances.GetByFrame(frame);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => await frame.GetNavigationService().NavigateAsync(page, parameter, infoOverride);

        public static async Task<bool> NavigateAsyncEx<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
            => await frame.GetNavigationService().NavigateAsync(key, parameter, infoOverride);
    }
}
