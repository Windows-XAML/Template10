using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.Foundation;

namespace Template10.Navigation
{
    public static class PageNavigationRegistry
    {
        static Dictionary<string, (Type PageType, Type ViewModelType)> _internal
            = new Dictionary<string, (Type PageType, Type ViewModelType)>();

        public static void Register(string pageKey, Type PageType, Type ViewModelType)
        {
            _internal.Add(pageKey, (PageType, ViewModelType));
        }

        public static void Register(string pageKey, Type PageType)
        {
            _internal.Add(pageKey, (PageType, null));
        }

        public static bool TryGetInfo(string pageKey, out (Type PageType, Type ViewModelType) info)
        {
            if (_internal.ContainsKey(pageKey))
            {
                info = _internal[pageKey];
                return true;
            }
            else
            {
                info = (null, null);
                return false;
            }
        }

        public static PageNavigationQueue ParsePath(string path, INavigationParameters parameters)
            => ParsePath(new Uri(path, UriKind.RelativeOrAbsolute), parameters);

        public static PageNavigationQueue ParsePath(Uri path, INavigationParameters parameters)
        {
            if (path.IsAbsoluteUri)
            {
                throw new ArgumentException($"{nameof(PageNavigationQueue)} only supports a relative Uri.");
            }
            var groups = path.ToString().Split("/")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select((x, index) => new PageNavigationInfo(x, parameters)
                {
                    Index = index,
                    Page = PageProvider?.Invoke(x.Split('?').First()),
                    ViewModel = ViewModelProvider?.Invoke(x.Split('?').First()),
                });
            return new PageNavigationQueue(groups)
            {
                ClearBackStack = path.ToString().StartsWith("/"),
            };
        }

        public static Func<string, Type> PageProvider { get; set; } = DefaultPageProvider;
        private static Type DefaultPageProvider(string pageKey)
        {
            if (TryGetInfo(pageKey, out var info) && info.PageType != null)
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

        public static Func<string, Type> ViewModelProvider { get; set; } = DefaultViewModelProvider;
        private static Type DefaultViewModelProvider(string pageKey)
        {
            if (TryGetInfo(pageKey, out var info) && info.ViewModelType != null)
            {
                return info.ViewModelType;
            }

            var assembly = pageKey.GetType().GetTypeInfo().AssemblyQualifiedName;
            var fullname = pageKey.GetType().FullName;
            var types = from space in new[] { string.Empty, "ViewModels." }
                        from suffix in new[] { string.Empty, "ViewModel" }
                        from page in new[] { pageKey, pageKey.Replace("Page", string.Empty), pageKey.Replace("View", string.Empty) }
                        select assembly.Replace(fullname, $"{pageKey.GetType().Namespace}.{space}{page}{suffix}");

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
