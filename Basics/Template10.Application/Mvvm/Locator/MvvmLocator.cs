using System;
using System.Linq;
using System.Reflection;
using Prism.Windows.Navigation;

namespace Prism.Windows.Mvvm
{
    public class MvvmLocator : IMvvmLocator
    {
        private IPageRegistry _registry;

        public MvvmLocator(IPageRegistry registry)
        {
            _registry = registry;
            FindView = DefaultFindView;
            FindViewModel = DefaultFindViewModel;
        }

        public Func<string, Type> FindView { get; set; }
        public Func<Type, Type> FindViewModel { get; set; }

        private Type DefaultFindView(string pageKey)
        {
            if (_registry.TryGetInfo(pageKey, out var info) && info.PageType != null)
            {
                return info.PageType;
            }

            var assembly = pageKey.GetType().GetTypeInfo().AssemblyQualifiedName;
            var fullname = pageKey.GetType().FullName;
            var types = from space in new[] { string.Empty, "Views.", "Pages." }
                        from suffix in new[] { string.Empty, "Page", "View" }
                        select assembly.Replace(fullname, $"{pageKey.GetType().Namespace}.{space}{pageKey}{suffix}");

            foreach (var type in types)
            {
                try
                {
                    var result = Type.GetType(type);
                    if (result == null)
                    {
                        continue;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return null;
        }

        private Type DefaultFindViewModel(Type page)
        {
            var assembly = page.GetTypeInfo().AssemblyQualifiedName;
            var fullname = page.FullName;
            var name = page.Name;
            var types = from space in new[] { string.Empty, "ViewModels." }
                        from suffix in new[] { string.Empty, "ViewModel" }
                        from moniker in new[] { name, name.Replace("Page", string.Empty), name.Replace("View", string.Empty) }
                        select assembly.Replace(fullname, $"{page.Namespace}.{space}{moniker}{suffix}");

            foreach (var type in types.Distinct())
            {
                try
                {
                    var result = Type.GetType(type);
                    if (result == null)
                    {
                        continue;
                    }
                    else
                    {
                        return result;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return null;
        }
    }
}
