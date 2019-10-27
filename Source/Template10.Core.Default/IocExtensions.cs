using System;
using System.Linq;
using Prism.Ioc;
using Unity;
using Unity.Resolution;

namespace Template10
{
    public static class IocExtensions
    {
        public static T Resolve<T>(this IContainerProvider containerProvider, Type type, params (Type Type, object Instance)[] parameters)
        {
            var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
            return (T)containerProvider.GetContainer().Resolve(type, overrides);
        }

        public static T Resolve<T>(this IContainerProvider containerProvider, Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
            return (T)containerProvider.GetContainer().Resolve(type, name, overrides);
        }

        public static bool IsRegistered(this IContainerProvider containerProvider, Type type)
        {
            return containerProvider.GetContainer().IsRegistered(type);
        }

        public static bool IsRegistered(this IContainerProvider containerProvider, Type type, string name)
        {
            return containerProvider.GetContainer().IsRegistered(type, name);
        }

        public static IUnityContainer GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IUnityContainer>)containerProvider).Instance;
        }

        public static IUnityContainer GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IUnityContainer>)containerRegistry).Instance;
        }
    }
}
