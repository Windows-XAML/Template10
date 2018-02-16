using Prism.Ioc;
using Prism.Navigation;
using Prism.Windows.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.Foundation;

namespace Prism.Windows.Navigation
{
    public class PageRegistry : IPageRegistry
    {
        static Dictionary<string, (Type View, Type ViewModel)> _cache
            = new Dictionary<string, (Type View, Type ViewModel)>();

        internal PageRegistry()
        {
            ViewFactory = DefaultViewFactory;
            ViewModelFactory = DefaultViewModelFactory;
        }

        public void Register(string key, (Type View, Type ViewModel) info)
        {
            _cache.Add(key, info);
        }

        public bool TryGetRegistration(string key, out (string Key, Type View, Type ViewModel) info)
        {
            if (_cache.ContainsKey(key))
            {
                info = (key, _cache[key].View, _cache[key].ViewModel);
                return true;
            }
            return TryUseFactories(key, out info);
        }

        public bool TryGetRegistration(Type view, out (string Key, Type View, Type ViewModel) info)
        {
            if (_cache.Any(x => x.Value.View == view))
            {
                var item = _cache.FirstOrDefault(x => x.Value.View == view);
                info = (item.Key, view, item.Value.ViewModel);
                return true;
            }
            return TryUseFactories(view.Name, out info);
        }

        public Func<string, Type> ViewFactory { get; set; }

        public Func<Type, Type> ViewModelFactory { get; set; }

        bool TryUseFactories(string key, out (string Key, Type View, Type ViewModel) info)
        {
            var view = ViewFactory(key);
            if (view == null)
            {
                info = (key, null, null);
                return false;
            }
            else
            {
                info = (key, view, ViewModelFactory(view));
                return true;
            }
        }

        private Type DefaultViewFactory(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var assembly = ReflectionUtilities.GetCallingAssembly(GetType().GetTypeInfo().Assembly);
            var assembly_name = assembly.FullName.Split(",").First();
            var types = from prefix in new[] { string.Empty, "Views.", "Pages." }
                        from suffix in new[] { string.Empty, "Page", "View" }
                        select $"{assembly_name}.{prefix}{key}{suffix}, {assembly.FullName}";
            var type = types.Select(x => Type.GetType(x)).Where(x => x != null).FirstOrDefault();
            return type;
        }

        private Type DefaultViewModelFactory(Type page)
        {
            if (page == null)
            {
                return null;
            }

            var name = page.Name;
            var assembly = ReflectionUtilities.GetCallingAssembly(GetType().GetTypeInfo().Assembly);
            var assembly_name = assembly.FullName.Split(",").First();
            var types = from prefix in new[] { string.Empty, "ViewModels." }
                        from suffix in new[] { string.Empty, "ViewModel" }
                        from moniker in new[] { name, name.Replace("Page", string.Empty), name.Replace("View", string.Empty) }
                        select $"{assembly_name}.{prefix}{moniker}{suffix}, {assembly.FullName}";
            var type = types.Select(x => Type.GetType(x)).Where(x => x != null).FirstOrDefault();
            return type;
        }
    }
}
