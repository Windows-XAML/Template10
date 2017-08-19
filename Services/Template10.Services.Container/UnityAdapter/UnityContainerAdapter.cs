using Microsoft.Practices.Unity;

namespace Template10.Services.Container
{
    public class UnityContainerAdapter : IContainerAdapter
    {
        Microsoft.Practices.Unity.IUnityContainer _container;
        internal UnityContainerAdapter()
        {
            _container = new Microsoft.Practices.Unity.UnityContainer();
        }

        Microsoft.Practices.Unity.LifetimeManager _lifetime
            => new Microsoft.Practices.Unity.ContainerControlledLifetimeManager();

        public T Resolve<T>() where T : class
            => _container.Resolve(typeof(T)) as T;

        public void Register<F, T>() where F : class where T : class, F
            => _container.RegisterType(typeof(F), typeof(T), _lifetime);

        public void Register<F>(F instance) where F : class
            => _container.RegisterInstance(typeof(F), instance, _lifetime);
    }
}
