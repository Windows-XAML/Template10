using Template10.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Navigation
{
    public static class NavigationFactory
    {
        /// <summary>
        /// Creates a navigation service
        /// </summary>
        /// <param name="gestures">Optional default getures tied to this Frame</param>
        /// <returns>INavigationService</returns>
        public static IPlatformNavigationService Create(params Gesture[] gestures)
        {
            return Create(new Frame(), Window.Current.CoreWindow, gestures);
        }

        /// <summary>
        /// Creates a navigation service
        /// </summary>
        /// <param name="frame">Required XAML Frame</param>
        /// <param name="gestures">Optional default getures tied to this Frame</param>
        /// <returns>INavigationService</returns>
        public static IPlatformNavigationService Create(Frame frame, params Gesture[] gestures)
        {
            return Create(frame, Window.Current.CoreWindow, gestures);
        }

        /// <summary>
        /// Creates a navigation service
        /// </summary>
        /// <param name="gestures">Optional default getures tied to this Frame</param>
        /// <returns>INavigationService</returns>
        public static IPlatformNavigationService Create(CoreWindow window, params Gesture[] gestures)
        {
            return Create(new Frame(), window, gestures);
        }

        /// <summary>
        /// Creates a navigation service
        /// </summary>
        /// <param name="frame">Required XAML Frame</param>
        /// <param name="gestures">Optional default getures tied to this Frame</param>
        /// <returns>INavigationService</returns>
        public static IPlatformNavigationService Create(Frame frame, CoreWindow window, params Gesture[] gestures)
        {
            frame = frame ?? new Frame();
            var gesture_service = GestureService.GetForCurrentView(window);
            var navigation_service = new NavigationService(frame);
            foreach (var gesture in gestures)
            {
                switch (gesture)
                {
                    case Gesture.Back:
                        gesture_service.BackRequested += async (s, e) => await navigation_service.GoBackAsync();
                        break;
                    case Gesture.Forward:
                        gesture_service.ForwardRequested += async (s, e) => await navigation_service.GoForwardAsync();
                        break;
                    case Gesture.Refresh:
                        gesture_service.RefreshRequested += async (s, e) => await navigation_service.RefreshAsync();
                        break;
                }
            }
            return navigation_service;
        }

        /// <summary>
        /// Creates navigation service
        /// </summary>
        /// <param name="frame">Pre-existing frame</param>
        /// <returns>INavigationService</returns>
        public static IPlatformNavigationService Create(Frame frame)
        {
            return new NavigationService(frame);
        }
    }
}
