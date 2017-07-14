using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Template10.Common;
using Template10.Services.WindowWrapper;

namespace Template10.Services.NavigationService
{
    public static class NavigationServiceHelper
    {
        private static object _PageKeys;
        public static Dictionary<T, Type> PageKeys<T>() where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if (_PageKeys != null && _PageKeys is Dictionary<T, Type>)
            {
                return _PageKeys as Dictionary<T, Type>;
            }
            return (_PageKeys = new Dictionary<T, Type>()) as Dictionary<T, Type>;
        }

        public static INavigationService Default { get; set; }
        public static NavigationServiceList Instances { get; } = new NavigationServiceList();
    }
}
