using System;

namespace Template10.Services.Dependency
{
    public abstract class DependencyService : IDependencyService
    {
        static IDependencyService _Default;

        protected IContainerAdapter Adapter { get; set; }

        public static IDependencyService Default
        {
            get => _Default;
            set => _Default = value;
        }

        public DependencyService(IContainerAdapter adapter, bool setAsDefault = true)
        {
            if (setAsDefault && _Default != null)
            {
                throw new System.Exception("Default ContainerService has already been set.");
            }
            Adapter = adapter;
            if (setAsDefault)
            {
                _Default = this;
            }
        }

        public TInterface Resolve<TInterface>(string key)
            where TInterface : class
            => Adapter.Resolve<TInterface>(key);

        public T Resolve<T>() where T : class
            => Adapter.Resolve<T>();

        public void Register<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
            => Adapter.Register<TInterface, TClass>(key);

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
            => Adapter.Register<TInterface, TClass>();

        public void RegisterInstance<TClass>(TClass instance)
            where TClass : class
            => Adapter.RegisterInstance<TClass>(instance);

        public void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface
            => Adapter.RegisterInstance<TInterface>(instance);
    }
}
