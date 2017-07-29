using Template10.Common;
using Template10.Core;
using Template10.Services.Container;
using Template10.Services.Messenger;
using Template10.Strategies;

namespace Template10
{
    public static partial class Settings
    {
        public static ITemplate10Dispatcher DefaultDispatcher { get; set; }

        static IMessengerService _MessengerService = Services.Messenger.MessengerService.Instance;
        public static IMessengerService MessengerService
        {
            get => _MessengerService ?? (_MessengerService = Services.Messenger.MessengerService.Instance);
            set => _MessengerService = value;
        }

        static IContainerService _ContainerService;
        public static IContainerService ContainerService
        {
            get => _ContainerService ?? (_ContainerService = Services.Container.ContainerService.GetInstance(UnityContainerAdapter.Create()));
            set => _ContainerService = value;
        }

        public static IBootStrapperStrategy BootStrapperStrategy { get; set; } = new DefaultBootStrapperStrategy();
        public static IExtendedSessionStrategy ExtendedSessionStrategy { get; set; } = new DefaultExtendedSessionStrategy();
        public static ILifecycleStrategy SuspendResumeStrategy { get; set; } = new DefaultLifecycleStrategy();
        public static ITitleBarStrategy TitleBarStrategy { get; set; } = new DefaultTitleBarStrategy();
    }
}
