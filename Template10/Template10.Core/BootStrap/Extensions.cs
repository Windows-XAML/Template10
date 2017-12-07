using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Navigation;

namespace Template10.Extensions
{
    public static class BootStrapperDependecyExtensions
    {
        public static TInterface Resolve<TInterface>(this Common.IBootStrapperDependecyInjection boot) where TInterface : class
        {
            return Central.DependencyService.Resolve<TInterface>();
        }

        public static TInterface Resolve<TInterface>(this Common.IBootStrapperDependecyInjection boot, string key) where TInterface : class
        {
            return Central.DependencyService.Resolve<TInterface>(key);
        }

        public static PageKeyRegistry  PageKeyRegistry(this Common.IBootStrapperDependecyInjection boot) 
        {
            return Navigation.Settings.PageKeyRegistry;
        }
    }
}
