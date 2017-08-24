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
        : BootStrap.BootStrapperBase
    {
        public sealed override IContainerService CreateDependecyContainer()
        {
            return new MvvmLightContainerService();
        }

        public sealed override void RegisterCustomDependencies()
        {
            Container.Register<IMessengerService, MvvmLightMessengerService>();
        }
    }
}
