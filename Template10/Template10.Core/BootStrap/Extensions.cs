using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Core;

namespace Template10.Extensions
{
    public static class BootStrapperDependecyExtensions
    {
        public static TInterface Resolve<TInterface>(this Core.IBootStrapperDependecyInjection boot) where TInterface : class
        {
            return Central.DependencyService.Resolve<TInterface>();
        }
        public static TInterface Resolve<TInterface>(this Core.IBootStrapperDependecyInjection boot, string key) where TInterface : class
        {
            return Central.DependencyService.Resolve<TInterface>(key);
        }
        public static IDictionary<TEnum, Type> PageKeys<TEnum>(this Core.IBootStrapperDependecyInjection boot) where TEnum : struct, IConvertible
        {
            return Navigation.Settings.PageKeys<TEnum>();
        }
    }
}
