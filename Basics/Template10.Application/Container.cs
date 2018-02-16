using Prism.Ioc;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Windows
{
    public static class Container
    {
        internal static IContainerExtension ContainerExtension { get; set; }
        internal static IContainerProvider ContainerProvider => ContainerExtension as IContainerProvider;
        internal static IContainerRegistry ContainerRegistry => ContainerExtension as IContainerRegistry;
        public static T Resolve<T>() => ContainerProvider.Resolve<T>();
        public static T Resolve<T>(string key) => ContainerProvider.Resolve<T>(key);
        public static object Resolve(Type type) => ContainerProvider.Resolve(type);
        public static object Resolve(Type type, string key) => ContainerProvider.Resolve(type, key);
        public static IPageRegistry PageRegistry { get; } = new PageRegistry();
    }
}
