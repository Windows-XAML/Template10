using System;

namespace Template10.Services.DependencyInjection
{
    public abstract class DependencyServiceBase : IDependencyService
    {
        static IDependencyService _current;

        protected IContainerAdapter Adapter { get; set; }

        public static IDependencyService Current
        {
            get => _current;
            set => _current = value;
        }

        public DependencyServiceBase(IContainerAdapter adapter, bool setAsCurrent = true)
        {
            if (setAsCurrent && _current != null)
            {
                throw new Exception("Current is alrady set.");
            }
            Adapter = adapter;
            if (setAsCurrent)
            {
                Current = this;
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
