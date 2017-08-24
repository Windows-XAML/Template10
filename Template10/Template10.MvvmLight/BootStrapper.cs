using Template10.Core;
using Template10.Services.Container;
using Template10.Services.Gesture;
using Template10.Services.Logging;
using Template10.Services.Messenger;
using Template10.Services.Resources;
using Template10.Services.Serialization;
using Template10.Strategies;

namespace Template10
{
    public abstract class BootStrapper
        : BootStrap.BootStrapperBase
    {
        public sealed override IContainerService CreateDependecyInjectionContainer()
        {
            return new MvvmLightContainerService();
        }

        public sealed override void RegisterCustomDependencies()
        {
            Container.Register<IMessengerService, MvvmLightMessengerService>();
        }
    }
}
