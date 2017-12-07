using Template10.BootStrap;
using Template10.Services.DependencyInjection;
using Template10.Services.Messaging;
using Template10.Services.Serialization;

namespace Template10
{
    public abstract class BootStrapper
        : BootStrapperBase
    {
        public sealed override IDependencyService CreateDependecyService()
        {
            return new DefaultDependencyService();
        }

        public sealed override void RegisterCustomDependencies(IDependencyService dependencyService)
        {
            dependencyService.Register<IMessengerService, DefaultMessengerService>();
            dependencyService.Register<ISerializationService, DefaultSerializationService>();
            SetupDependencies(dependencyService);
        }

        public virtual void SetupDependencies(IContainerBuilder containerBuilder) { /* empty */ }
    }
}

