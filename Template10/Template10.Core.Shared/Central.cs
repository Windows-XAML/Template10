using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static AppVisibilities AppVisibility { get; set; }
            = AppVisibilities.Unknown;

        public static Services.Container.IContainerService ContainerService
            => Services.Container.ContainerService.Default;

        public static Services.Messenger.IMessengerService MessengerService
            => ContainerService.Resolve<Services.Messenger.IMessengerService>();

        public static Services.Serialization.ISerializationService SerializationService
            => ContainerService.Resolve<Services.Serialization.ISerializationService>();

        public static Services.Resources.IResourceService ResourceService
            => ContainerService.Resolve<Services.Resources.ResourceService>();

        public static Services.Logging.ILoggingService LoggingService
            => ContainerService.Resolve<Services.Logging.ILoggingService>();

        public static Services.Gesture.IGestureService GestureService
            => ContainerService.Resolve<Services.Gesture.IGestureService>();

        public static Core.ISessionState SessionState
            => ContainerService.Resolve<Core.ISessionState>();

        public static Core.IBootStrapper BootStrapper
            => ContainerService.Resolve<Core.IBootStrapper>();
    }
}
