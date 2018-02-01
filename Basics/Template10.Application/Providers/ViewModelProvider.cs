using System;
using System.Linq;
using System.Reflection;

namespace Template10.Navigation
{
    public class ViewModelProvider : IViewModelProvider
    {
        public ViewModelProvider()
        {
            Provider = DefaultProvider;
        }

        public Func<string, Type> Provider { get; set; }

        private Type DefaultProvider(string pageKey)
        {
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
