using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Extensions
{
    public static class Extensions
    {
        public static TInterface Resolve<TInterface>(this Template10.Core.IBootStrapperDependecyInjection boot) where TInterface : class
        {
            return Central.Container.Resolve<TInterface>();
        }
        public static TInterface Resolve<TInterface>(this Template10.Core.IBootStrapperDependecyInjection boot, string key) where TInterface : class
        {
            return Central.Container.Resolve<TInterface>(key);
        }
        public static IDictionary<TEnum, Type> PageKeys<TEnum>(this Template10.Core.IBootStrapperDependecyInjection boot) where TEnum : struct, IConvertible
        {
            return Navigation.Settings.PageKeys<TEnum>();
        }
    }
}
