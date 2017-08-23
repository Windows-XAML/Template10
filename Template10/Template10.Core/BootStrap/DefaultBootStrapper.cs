using Template10.Services.Container;
using Template10.Services.Logging;
using Template10.Core;
using Template10.Strategies;
using Template10.Services.Serialization;
using Template10.Services.Gesture;
using Template10.Services.Resources;
using Template10.Services.Messenger;

namespace Template10.BootStrap
{
    public abstract class DefaultBootStrapper
        : Template10.BootStrap.BootStrapperBase
    {
        public DefaultBootStrapper(IContainerService containerService)
            : base(containerService)
        {

        }

        public sealed override void RegisterInternalDependencies()
        {
            // services
            ContainerService.Register<ISessionState, SessionState>();
            ContainerService.Register<ILoggingService, LoggingService>();
            ContainerService.Register<ISerializationService, JsonSerializationService>();
            ContainerService.Register<IBackButtonService, BackButtonService>();
            ContainerService.Register<IKeyboardService, KeyboardService>();
            ContainerService.Register<IGestureService, GestureService>();
            ContainerService.Register<IResourceService, ResourceService>();

#if DEBUG
            // test
            var mservice = ContainerService.Resolve<IMessengerService>();
            var sservice = ContainerService.Resolve<ISessionState>();
            var lservice = ContainerService.Resolve<ILoggingService>();
            var xservice = ContainerService.Resolve<ISerializationService>();
            var bservice = ContainerService.Resolve<IBackButtonService>();
            var kservice = ContainerService.Resolve<IKeyboardService>();
            var gservice = ContainerService.Resolve<IGestureService>();
            var rservice = ContainerService.Resolve<IResourceService>();
#endif

            // strategies
            ContainerService.RegisterInstance<IBootStrapperShared>(this);
            ContainerService.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            ContainerService.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            ContainerService.Register<IStateStrategy, DefaultStateStrategy>();
            ContainerService.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            ContainerService.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            ContainerService.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            ContainerService.Register<IViewModelResolutionStrategy, DefaultViewModelResolutionStrategy>();

#if DEBUG 
            // test
            var bstrategy = ContainerService.Resolve<IBootStrapperStrategy>();
            var lstrategy = ContainerService.Resolve<ILifecycleStrategy>();
            var sstrategy = ContainerService.Resolve<IStateStrategy>();
            var tstrategy = ContainerService.Resolve<ITitleBarStrategy>();
            var estrategy = ContainerService.Resolve<IExtendedSessionStrategy>();
            var astrategy = ContainerService.Resolve<IViewModelActionStrategy>();
            var rstrategy = ContainerService.Resolve<IViewModelResolutionStrategy>();
#endif
        }
    }
}
