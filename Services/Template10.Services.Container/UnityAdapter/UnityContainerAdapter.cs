using Microsoft.Practices.Unity;

namespace Template10.Services.Container
{
    public class UnityContainerAdapter : IContainerAdapter
    {
        IUnityContainer _container;
        internal UnityContainerAdapter()
        {
            _container = new UnityContainer();
        }

        LifetimeManager _lifetime => new ContainerControlledLifetimeManager();

        public TInterface Resolve<TInterface>() where TInterface : class
            => _container.Resolve<TInterface>();

        public TInterface Resolve<TInterface>(string key) where TInterface : class
            => _container.Resolve<TInterface>(name: key);

        public void Register<TIntreface, TClass>()
            where TIntreface : class
            where TClass : class, TIntreface
            => _container.RegisterType<TIntreface, TClass>(_lifetime);

        public void Register<TInterface, TClass>(string key)
            where TInterface : class 
            where TClass : class, TInterface
            => _container.RegisterType<TInterface, TClass>(name: key, lifetimeManager: _lifetime);

        public void Register<TClass>(TClass instance) where TClass : class
            => _container.RegisterInstance(instance, _lifetime);
    }
}
