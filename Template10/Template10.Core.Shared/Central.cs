using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Core;

namespace Template10
{
    public enum AppVisibilities
    {
        Foreground,
        Background,
        Unknown
    }

    public static class Central
    {
        public static AppVisibilities Visibility { get; set; }
            = AppVisibilities.Unknown;

        public static ISessionState SessionState
            => Container.Resolve<ISessionState>();

        public static Template10.Services.Container.IContainerService Container
            => Template10.Services.Container.ContainerService.Default;

        public static Template10.Services.Messenger.IMessengerService Messenger
            => Container.Resolve<Template10.Services.Messenger.IMessengerService>();

        public static Template10.Services.Serialization.ISerializationService Serialization
            => Container.Resolve<Template10.Services.Serialization.ISerializationService>();

        public static Template10.Services.Resources.IResourceService Resource
            => Container.Resolve<Template10.Services.Resources.ResourceService>();

        public static Template10.Services.Network.INetworkAvailableService Network
            => Container.Resolve<Template10.Services.Network.INetworkAvailableService>();

        public static Template10.Services.Logging.ILoggingService Logging
            => Container.Resolve<Template10.Services.Logging.ILoggingService>();

        public static Template10.Services.Gesture.IGestureService Gesture
            => Container.Resolve<Template10.Services.Gesture.IGestureService>();

        public static IWindowEx Window
            => WindowManager.Instances.First(x => x.Window.CoreWindow == Windows.UI.Core.CoreWindow.GetForCurrentThread());

        public static IDispatcherEx Dispatcher
            => Window.Dispatcher;
    }
}
