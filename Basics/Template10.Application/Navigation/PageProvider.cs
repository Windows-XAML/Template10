using System;
using System.Linq;
using System.Reflection;

namespace Template10.Navigation
{
    public class PageProvider: IPageProvider
    {
        public PageProvider()
        {
            Provider = DefaultProvider;
        }

        public Func<string, Type> Provider { get; set; }

        private Type DefaultProvider(string pageKey)
        {
            if (PageNavigationRegistry.TryGetInfo(pageKey, out var info) && info.PageType != null)
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
    }
}
