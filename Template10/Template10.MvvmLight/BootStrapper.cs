using Template10.BootStrap;
using Template10.Services.DependencyInjection;
using Template10.Services.Messaging;
using Template10.Services.Serialization;
using Template10.Strategies;

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
            // required services
            dependencyService.Register<IMessengerService, DefaultMessengerService>();
            dependencyService.Register<ISerializationService, DefaultSerializationService>();

            // required strategies
            dependencyService.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            dependencyService.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            dependencyService.Register<INavStateStrategy, DefaultNavStateStrategy>();
            dependencyService.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            dependencyService.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            dependencyService.Register<IViewModelResolutionStrategy, DefaultViewModelResolutionStrategy>();

            // custom
            RegisterDependencies(dependencyService);
        }

        public virtual void RegisterDependencies(IContainerBuilder containerBuilder) { /* empty */ }
    }
}

