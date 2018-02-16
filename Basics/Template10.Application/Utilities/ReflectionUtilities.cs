using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Prism.Windows.Utilities
{
    public static class ReflectionUtilities
    {
        public static IEnumerable<Assembly> GetCallingAssemblies()
        {
            var s = new StackTrace();
            return s.GetFrames()
                .Select(x => x.GetMethod().DeclaringType.GetTypeInfo().Assembly)
                .Where(x => !x.FullName.StartsWith("Microsoft."))
                .Where(x => !x.FullName.StartsWith("System."));
        }

        public static Assembly GetCallingAssembly(Assembly ignore)
        {
            return GetCallingAssemblies()
                .Where(x => !Equals(x, ignore))
                .First();
        }
    }
}
