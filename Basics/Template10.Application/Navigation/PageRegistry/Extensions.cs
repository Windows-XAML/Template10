using Prism.Ioc;

namespace Prism.Windows.Navigation
{
    public static partial class Extensions
    {
        private static IPageRegistry _registry;

        static Extensions()
        {
            _registry = PrismApplicationBase.Container.Resolve<IPageRegistry>();
        }

        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry)
        {
            RegisterForNavigation<TView, TViewModel>(registry, typeof(TView).Name);
        }

        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry, string key)
        {
            registry.Register<TViewModel>(key);
            RegisterForNavigation<TView>(registry, key);
        }

        public static void RegisterForNavigation<TView>(this IContainerRegistry registry)
        {
            RegisterForNavigation<TView>(registry, typeof(TView).Name);
        }

        public static void RegisterForNavigation<TView>(this IContainerRegistry registry, string key)
        {
            _registry.Register(key, typeof(TView), null);
        }
    }
}
