using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Core.Services;
using Template10.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Navigation
{
    public static class INavigationServiceExtensions
    {
        public static Frame GetXamlFrame(this INavigationService service)
        {
            return (service.GetFrameFacade() as IFrameFacade2).Frame;
        }

        public static IFrameFacade GetFrameFacade(this INavigationService service)
        {
            return (service as INavigationService2).FrameFacade;
        }

        public static INavigationService ActivateWindow(this INavigationService service, Window window = null)
        {
            var frame = service.GetXamlFrame();
            window = window ?? Window.Current;
            window.Content = frame;
            window.Activate();
            while (!WindowService.TryGetWindow(frame, out _))
            {
                Task.Delay(50).RunSynchronously();
            }
            return service;
        }

        public static INavigationService AttachGestures(this INavigationService service, params Gesture[] gestures)
        {
            var frame = service.GetXamlFrame();
            if (!WindowService.TryGetWindow(frame, out var window))
            {
                throw new Exception("XAML Frame has is not part of a Window's visual tree. Gestures require a CoreWindow.");
            }
            return AttachGestures(service, window.CoreWindow, gestures);
        }

        public static INavigationService AttachGestures(this INavigationService service, CoreWindow window, params Gesture[] gestures)
        {
            var service2 = service as INavigationService2;
            var gesture_service = GestureService.GetForCurrentView(window);
            foreach (var gesture in gestures)
            {
                switch (gesture)
                {
                    case Gesture.Back:
                        AddOnlyOne(gesture_service.BackRequestedCallbacks, service2.FrameFacade.Id, async () => await service.GoBackAsync());
                        break;
                    case Gesture.Forward:
                        AddOnlyOne(gesture_service.ForwardRequestedCallbacks, service2.FrameFacade.Id, async () => await service.GoForwardAsync());
                        break;
                    case Gesture.Refresh:
                        AddOnlyOne(gesture_service.RefreshRequestedCallbacks, service2.FrameFacade.Id, async () => await service.RefreshAsync());
                        break;
                }
            }
            return service;

            void AddOnlyOne(Dictionary<string, Action> dictionary, string id, Action action)
            {
                dictionary.Remove(id);
                dictionary.Add(id, action);
            }
        }
    }
}
