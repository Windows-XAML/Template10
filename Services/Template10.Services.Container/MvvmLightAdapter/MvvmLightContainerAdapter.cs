using System;
using System.Collections.Generic;

namespace Template10.Services.Container
{
    public class MvvmLightContainerAdapter : IContainerAdapter
    {
        GalaSoft.MvvmLight.Ioc.SimpleIoc _container;
        internal MvvmLightContainerAdapter()
        {
            _container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
        }

        public void Register<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
        {
            // note: this is a workaround until SimpleIoT supports .Register<TInterface, TClass>(string key);
            _container.Register<TClass>();
        }

        public TInterface Resolve<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
        {
            // note: this is a workaround until SimpleIoT supports .Register<TInterface, TClass>(string key);
            return _container.IsRegistered<TClass>() 
                ? _container.GetInstance<TClass>() 
                : throw new Exception($"The type [{typeof(TClass)}] is not registered.");
        }

        public TInterface Resolve<TInterface>()
            where TInterface : class
            => _container.GetInstance<TInterface>() as TInterface;

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
            => _container.Register<TInterface, TClass>();

        public void RegisterInstance<TClass>(TClass instance)
            where TClass : class
            => _container.Register<TClass>(() => instance);

        public void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface
            => _container.Register<TInterface>(() => instance);

    }
}
