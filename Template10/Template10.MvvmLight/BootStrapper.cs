using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Services.Container;
using Template10.Services.Logging;
using Template10.Navigation;
using Template10.Core;
using Template10.Strategies;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using static Template10.Core.StartArgsEx;
using Template10.Services.Messenger;
using Template10.Services.Serialization;
using Template10.Services.Gesture;
using Template10.Messages;
using Template10.Services.Resources;

namespace Template10
{
    public abstract class BootStrapper
        : Template10.BootStrap.BootStrapperBase
    {
        public sealed override void RegisterDependencies()
        {
            // container
            var c = Services.Container.ContainerService.Default = new MvvmLightContainerService();

            // services
            c.Register<IMessengerService, MvvmLightMessengerService>();
            c.Register<ISessionState, SessionState>();
            c.Register<ILoggingService, LoggingService>();
            c.Register<ISerializationService, JsonSerializationService>();
            c.Register<IBackButtonService, BackButtonService>();
            c.Register<IKeyboardService, KeyboardService>();
            c.Register<IGestureService, GestureService>();
            c.Register<IResourceService, ResourceService>();

#if DEBUG
            // test
            var mservice = c.Resolve<IMessengerService>();
            var sservice = c.Resolve<ISessionState>();
            var lservice = c.Resolve<ILoggingService>();
            var xservice = c.Resolve<ISerializationService>();
            var bservice = c.Resolve<IBackButtonService>();
            var kservice = c.Resolve<IKeyboardService>();
            var gservice = c.Resolve<IGestureService>();
            var rservice = c.Resolve<IResourceService>();
#endif

            // strategies
            c.Register<IBootStrapperShared>(this);
            c.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            c.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            c.Register<IStateStrategy, DefaultStateStrategy>();
            c.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            c.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            c.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            c.Register<IViewModelResolutionStrategy, DefaultViewModelResolutionStrategy>();

#if DEBUG 
            // test
            var bstrategy = c.Resolve<IBootStrapperStrategy>();
            var lstrategy = c.Resolve<ILifecycleStrategy>();
            var sstrategy = c.Resolve<IStateStrategy>();
            var tstrategy = c.Resolve<ITitleBarStrategy>();
            var estrategy = c.Resolve<IExtendedSessionStrategy>();
            var astrategy = c.Resolve<IViewModelActionStrategy>();
            var rstrategy = c.Resolve<IViewModelResolutionStrategy>();
#endif
        }
    }
}
