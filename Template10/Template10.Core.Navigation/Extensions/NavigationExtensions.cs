using System;
using System.Threading.Tasks;
using Template10.Core;
using Template10.Mvvm;
using Template10.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Template10.Services.Serialization;

namespace Template10.Extensions
{
    public static class NavigationExtensions
    {
        public static IWindowEx GetWindow(this INavigationService service)
            => service.Window;

        public static IDispatcherEx GetDispatcher(this INavigationService service)
            => service.GetWindow().Dispatcher;

        public static async void CreateNavigationService(this Frame frame, string frameId = "RootFrame", BackButton backButton = BackButton.Attach)
            => (await NavigationService.CreateAsync(BackButton.Attach, frame)).FrameEx.FrameId = frameId;

        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationService.Instances.GetByFrame(frame);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => await frame.GetNavigationService().NavigateAsync(page, parameter, infoOverride);

        public static async Task<bool> NavigateAsyncEx<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
            => await frame.GetNavigationService().NavigateAsync(key, parameter, infoOverride);

        public static NavMode ToPortableNavigationMode(this NavigationMode mode)
        {
            switch (mode)
            {
                case NavigationMode.New: return NavMode.New;
                case NavigationMode.Back: return NavMode.Back;
                case NavigationMode.Forward: return NavMode.Forward;
                case NavigationMode.Refresh: return NavMode.Refresh;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
