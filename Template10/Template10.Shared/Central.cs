using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10
{
    public static class Central
    {
        public static Services.Container.IContainerService ContainerService 
            => Services.Container.ContainerService.Default;

        public static Services.Messenger.IMessengerService MessengerService 
            => ContainerService.Resolve<Services.Messenger.IMessengerService>();

        public static Services.Serialization.ISerializationService SerializationService 
            => ContainerService.Resolve<Services.Serialization.ISerializationService>();

        public static Services.Logging.ILoggingService LoggingService 
            => ContainerService.Resolve<Services.Logging.ILoggingService>();

        public static Services.BackButtonService.IBackButtonService BackButtonService
            => ContainerService.Resolve<Services.BackButtonService.IBackButtonService>();

        public static Core.ISessionState SessionState
            => ContainerService.Resolve<Core.ISessionState>();
    }
}
