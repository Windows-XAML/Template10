using System;
using System.Linq;
using Prism.Ioc;
using Unity;
using Unity.Resolution;

namespace Template10
{
    public sealed class UnityContainerExtension : IContainerExtension<IUnityContainer>
    {
        public UnityContainerExtension(IUnityContainer container) => Instance = container;

        public IUnityContainer Instance { get; }

        public void FinalizeExtension() { /* empty */ }

        public bool IsRegistered(Type type)
        {
            return Instance.IsRegistered(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            return Instance.IsRegistered(type, name);
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.RegisterType(from, to);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.RegisterType(from, to, name);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.RegisterInstance(type, instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.RegisterInstance(type, name, instance);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.RegisterSingleton(from, to);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.RegisterSingleton(from, to, name);
            return this;
        }

        public object Resolve(Type type)
        {
            return Instance.Resolve(type);
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
            return Instance.Resolve(type, overrides);
        }

        public object Resolve(Type type, string name)
        {
            return Instance.Resolve(type, name);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
            return Instance.Resolve(type, name, overrides);
        }

        //public object ResolveViewModelForView(object view, Type viewModelType)
        //{
        //    if (view is Page page)
        //    {
        //        var service = NavigationService.Instances[page.Frame];
        //        var overrides = new ResolverOverride[]
        //        {
        //            new DependencyOverride(typeof(INavigationService), service)
        //        };
        //        return Instance.Resolve(viewModelType, overrides);
        //    }
        //    else
        //    {
        //        return Instance.Resolve(viewModelType);
        //    }
        //}
    }

    //public static class UnityContainerExtensions
    //{

    //    public static T Resolve<T>(this IContainerProvider provider)
    //    {
    //        return (T)provider.Resolve(typeof(T));
    //    }

    //    public static T Resolve<T>(this IContainerProvider provider, params (Type Type, object Instance)[] parameters)
    //    {
    //        return (T)provider.Resolve(typeof(T), parameters);
    //    }

    //    public static T Resolve<T>(this IContainerProvider provider, string name, params (Type Type, object Instance)[] parameters)
    //    {
    //        return (T)provider.Resolve(typeof(T), name, parameters);
    //    }

    //    public static T Resolve<T>(this IContainerProvider provider, string name)
    //    {
    //        return (T)provider.Resolve(typeof(T), name);
    //    }
    //}
}
