using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Template10.Services.Dependency
{
    public class MvvmLightContainerAdapter : IContainerAdapter
    {
        internal GalaSoft.MvvmLight.Ioc.SimpleIoc _container;
        internal MvvmLightContainerAdapter()
        {
            _container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
        }

        IDictionary<string, Type> key_map = new Dictionary<string, Type>();

        public void Register<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
        {
            if (key_map.ContainsKey(key))
            {
                throw new Exception($"Container.Register<{typeof(TInterface).Name},{typeof(TClass).Name}({key}) " +
                    $"but, this key is already registered.");
            }
            _container.Register<TClass>();
            key_map.Add(key, typeof(TClass));
        }

        public TInterface Resolve<TInterface>(string key)
            where TInterface : class
        {
            if (!key_map.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Cannot resolve to {typeof(TInterface)} " +
                    $"because key[{key}] is not registered.");
            }
            var result = _container.GetInstance(key_map[key]);
            if (!(result is TInterface))
            {
                throw new InvalidCastException($"Cannot resolve key[{key}] " +
                    $"because {result.GetType()} cannot cast to {typeof(TInterface)}.");
            }
            return (TInterface)result;
        }

        public TInterface Resolve<TInterface>()
            where TInterface : class
        {
            try
            {
                if (_container.GetInstance<TInterface>() is TInterface t)
                {
                    if (t == null)
                    {
                        throw new InvalidCastException($"Resolve() failed for {typeof(TInterface)}");
                    }
                    else
                    {
                        return t;
                    }
                }
                else
                {
                    throw new InvalidCastException($"Resolve() failed for {typeof(TInterface)}");
                }
            }
            catch(Exception ex)
            {
                throw new InvalidCastException($"Resolve() failed for {typeof(TInterface)}", ex);
            }
        }

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
        {
            if (_container.IsRegistered<TInterface>())
            {
                _container.Unregister<TInterface>();
            }
            _container.Register<TInterface, TClass>();
        }

        public void RegisterInstance<TClass>(TClass instance)
            where TClass : class
            => _container.Register<TClass>(() => instance);

        public void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface
            => _container.Register<TInterface>(() => instance);

        TInterface QuickResolve<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
        {
            try
            {
                _container.Register<TInterface, TClass>();
                return _container.GetInstance<TInterface>();
            }
            catch { throw; }
            finally { _container.Unregister<TInterface>(); }
        }
    }
}
