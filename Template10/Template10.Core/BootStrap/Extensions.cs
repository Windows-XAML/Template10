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
        /// <summary>
        /// Resolve an object registered with the Dependency Container.
        /// </summary>
        /// <typeparam name="TInterface">The type (generally interface) of the desired object</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TInterface Resolve<TInterface>(this object obj) where TInterface : class
        {
            return Central.DependencyService.Resolve<TInterface>();
        }

        /// <summary>
        /// Resolve an object registered with the Dependency Container.
        /// </summary>
        /// <typeparam name="TInterface">The type (generally interface) of the desired object</typeparam>
        /// <param name="obj"></param>
        /// <param name="key">Specific key, if keys were used during registration</param>
        /// <returns></returns>
        public static TInterface Resolve<TInterface>(this object obj, string key) where TInterface : class
        {
            return Central.DependencyService.Resolve<TInterface>(key);
        }
    }
}
