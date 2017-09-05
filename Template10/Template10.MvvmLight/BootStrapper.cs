using System;
using System.Collections.Generic;
using Template10.BootStrap;
using Template10.Services.Container;
using Template10.Services.Messenger;

namespace Template10
{
    public abstract class BootStrapper
        : BootStrapperBase
    {
        public sealed override IContainerService CreateDependecyContainer()
        {
            return new MvvmLightContainerService();
        }

        public sealed override void RegisterCustomDependencies(IContainerBuilder container)
        {
            container.Register<IMessengerService, MvvmLightMessengerService>();
            SetupDependencies(container);
        }

        public virtual void SetupDependencies(IContainerBuilder container) { /* empty */ }
    }
}

