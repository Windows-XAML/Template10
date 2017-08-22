using System;
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

        public T Resolve<T>()
            where T : class
            => _container.Resolve(typeof(T)) as T;

        public TInterface Resolve<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
            => _container.Resolve<TInterface>(key);

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
            => _container.RegisterType(typeof(TInterface), typeof(TClass), _lifetime);

        public void Register<TInterface>(TInterface instance)
            where TInterface : class
            => _container.RegisterInstance(typeof(TInterface), instance, _lifetime);

        void IContainerAdapter.Register<TInterface, TClass>(string key)
            => _container.RegisterType<TInterface, TClass>(key);

        public void RegisterInstance<TClass>(TClass instance)
            where TClass : class
            => _container.RegisterInstance(instance);

        void IContainerAdapter.RegisterInstance<TInterface, TClass>(TClass instance)
            => _container.RegisterInstance<TInterface>(instance);
    }
}
