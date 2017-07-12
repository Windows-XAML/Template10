using System.Collections.Generic;

namespace Template10.Common
{
    public static partial class Extensions
    {
        public static void SetValue(this IDictionary<string, object> dictionary, string key, object value)
        {
            dictionary[key] = value;
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            try
            {
                if (dictionary.TryGetValue(key, out var result))
                {
                    value = (T)result;
                    return true;
                }
                else
                {
                    value = default(T);
                    return false;
                }
            }
            catch
            {
                value = default(T);
                return false;
            }
        }
    }
}
