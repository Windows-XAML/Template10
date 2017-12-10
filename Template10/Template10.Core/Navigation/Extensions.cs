using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Extensions
{
    public static class NavigationExtensions
    {
        public static INavigationService RegisterNavigationService(this Frame frame, BackButton button = BackButton.Attach)
        {
            if (frame.GetNavigationService() != null)
            {
                throw new Exception("Frame is already registered.");
            }
            return NavigationService.Create(button, frame);
        }

        public static IWindowEx GetWindow(this INavigationService service)
            => service.Window;

        public static IDispatcherEx GetDispatcher(this INavigationService service)
            => service.GetWindow().Dispatcher;

        public static void CreateNavigationService(this Frame frame, string frameId = "RootFrame", BackButton backButton = BackButton.Attach)
            => NavigationService.Create(BackButton.Attach, frame).FrameEx.FrameId = frameId;

        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationService.Instances.GetByFrame(frame);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => await frame.GetNavigationService().NavigateAsync(page, parameter, infoOverride);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, string key, object parameter = null, NavigationTransitionInfo infoOverride = null)
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
