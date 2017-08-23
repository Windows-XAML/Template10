using System;
using Template10.BootStrap;
using Template10.Services.Container;
using Template10.Services.Messenger;

namespace Template10
{
    public abstract class MvvmLightBootStrapper
        : DefaultBootStrapper
    {
        public MvvmLightBootStrapper() : base(new MvvmLightContainerService())
        {
        }

        public override void RegisterMessagingService()
        {
            ContainerService.Register<IMessengerService, MvvmLightMessengerService>();
        }
    }
}
