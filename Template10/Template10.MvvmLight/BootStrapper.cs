using System;
using System.Collections.Generic;
using Template10.BootStrap;
using Template10.Services.Dependency;
using Template10.Services.Messenger;

namespace Template10
{
    public abstract class BootStrapper
        : BootStrapperBase
    {
        public sealed override IDependencyService CreateDependecyService()
        {
            return new MvvmLightDependencyService();
        }

        public sealed override void RegisterCustomDependencies(IDependencyService dependencyService)
        {
            dependencyService.Register<IMessengerService, MvvmLightMessengerService>();
            SetupDependencies(dependencyService);
        }

        public virtual void SetupDependencies(IContainerBuilder containerBuilder) { /* empty */ }
    }
}

