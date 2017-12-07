using System;
using System.Collections.Generic;
using System.Diagnostics;
using Template10.Services.DependencyInjection;

namespace Template10.Impl
{
    public partial class DependencyService : IDependencyService
    {
        private GalaSoft.MvvmLight.Ioc.SimpleIoc _container;
        public DependencyService()
        {
            _container = new GalaSoft.MvvmLight.Ioc.SimpleIoc();
            Settings.Current = this;
        }

        IDictionary<string, Type> _key_map = new Dictionary<string, Type>();

        public void Register<TInterface, TClass>(string key)
            where TInterface : class
            where TClass : class, TInterface
        {
            Debug.WriteLine($"Container registering instance {typeof(TClass)} as {typeof(TInterface)} with key {key}.");
            if (_key_map.ContainsKey(key))
            {
                throw new Exception($"Container.Register<{typeof(TInterface).Name},{typeof(TClass).Name}({key}) " +
                    $"but, this key is already registered.");
            }
            _key_map.Add(key, typeof(TClass));
            _container.Register<TClass>();
        }

        public TInterface Resolve<TInterface>(string key)
            where TInterface : class
        {
            Debug.WriteLine($"Container resolving {typeof(TInterface)} with key {key}.");
            if (!_key_map.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Cannot resolve to {typeof(TInterface)} " +
                    $"because key[{key}] is not registered.");
            }
            var type = _key_map[key];
            var result = _container.GetInstance(type);
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
            Debug.WriteLine($"Container resolving {typeof(TInterface)}.");
            try
            {
                if (_container.GetInstance<TInterface>() is TInterface item)
                {
                    if (item == null)
                    {
                        throw new InvalidCastException($"Resolve() failed for {typeof(TInterface)}");
                    }
                    else
                    {
                        return item;
                    }
                }
                else
                {
                    throw new InvalidCastException($"Resolve() failed for {typeof(TInterface)}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Resolve() failed for {typeof(TInterface)}", ex);
            }
        }

        public void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
        {
            Debug.WriteLine($"Container registering {typeof(TClass)} as type {typeof(TInterface)}.");
            if (_container.IsRegistered<TInterface>())
            {
                Debug.WriteLine($"Unregistering pre-existing registation {typeof(TClass)} as {typeof(TInterface)}.");
                _container.Unregister<TInterface>();
            }
            _container.Register<TInterface, TClass>();
        }

        public void RegisterInstance<TClass>(TClass instance)
            where TClass : class
        {
            Debug.WriteLine($"Container registering instance {typeof(TClass)}.");
            _container.Register<TClass>(() => instance);
        }

        public void RegisterInstance<TInterface, TClass>(TClass instance)
            where TInterface : class
            where TClass : class, TInterface
        {
            Debug.WriteLine($"Container registering instance {typeof(TClass)} as {typeof(TInterface)}.");
            _container.Register<TInterface>(() => instance);
        }

        TInterface QuickResolve<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
        {
            Debug.WriteLine($"Container quick-resolving {typeof(TClass)} as {typeof(TInterface)}. Type must not be registered.");
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
