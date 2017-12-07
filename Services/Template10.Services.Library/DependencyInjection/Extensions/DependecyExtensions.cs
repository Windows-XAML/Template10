using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Extensions
{
    internal static class DependecyExtensions
    {
        public static TInterface Resolve<TInterface>() where TInterface : class
        {
            return Services.DependencyInjection.Settings.Current.Resolve<TInterface>();
        }
    }
}
