using System;
using System.Collections.Generic;
using Template10.Services.Container;
using Template10.Services.Messenger;

namespace Template10.Core
{
    public interface IBootStrapperShared
    {
        IDictionary<string, object> SessionState { get; }
        IMessengerService MessengerService { get; }
        IContainerService ContainerService { get; }
        IDispatcherEx Dispatcher { get; }
    }
}
