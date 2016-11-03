using System;
using System.Collections.Generic;
using System.Reflection;

namespace Template10.App
{
    public static partial class Settings
    {
        public static bool AutoRestore { get; set; } = true;
        public static bool AutoExtendExecution { get; set; } = true;
        public static bool AutoSuspend { get; set; } = true;
        public static bool LogginEnabled { get; set; } = false;
        public static TimeSpan SuspensionStateExpires { get; set; } = TimeSpan.FromDays(3);

        private static object _PageKeys;
        public static Dictionary<T, Type> PageKeys<T>()
            where T : struct, IConvertible
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
    }
}