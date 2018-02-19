using Prism.Ioc;
using System;

namespace Prism.Windows.Navigation
{
    public static partial class Extensions
    {
        public static void Register(IContainerRegistry registry, string key, Type view, Type viewModel)
        {
            if (viewModel != null)
            {
                registry.Register(viewModel);
            }
            PageRegistry.Register(key, (view, viewModel));
        }

        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry)
            => Register(registry, typeof(TView).Name, typeof(TView), typeof(TViewModel));
        public static void RegisterForNavigation<TView, TViewModel>(this IContainerRegistry registry, string key)
            => Register(registry, key, typeof(TView), typeof(TViewModel));
        public static void RegisterForNavigation<TView>(this IContainerRegistry registry)
            => Register(registry, typeof(TView).Name, typeof(TView), null);
        public static void RegisterForNavigation<TView>(this IContainerRegistry registry, string key)
            => Register(registry, key, typeof(TView), null);
    }
}
