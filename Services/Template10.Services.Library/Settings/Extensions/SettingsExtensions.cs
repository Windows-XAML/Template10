namespace Template10.Extensions
{
    using System;
    using Template10.Services.Serialization;
    using Windows.Storage;
    public static class SettingsExtensions
    {
        static ISerializationService GetDefaultSerializer()
        {
            return DependecyExtensions.Resolve<ISerializationService>();
        }

        //[Obsolete]
        public static bool TryRead<T>(this ApplicationDataContainer container, string key, out T value, bool decompress = true, ISerializationService serializer = null)
        {
            if (!container.Values.ContainsKey(key))
            {
                value = default(T);
                return false;
            }
            try
            {
                var result = container.Values[key]?.ToString();
                result = decompress ? result.Decompress() : result;
                serializer = serializer ?? GetDefaultSerializer();
                value = serializer.Deserialize<T>(result);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        //[Obsolete]
        public static bool TryWrite<T>(this ApplicationDataContainer container, string key, T value, bool compress = true, ISerializationService serializer = null)
        {
            try
            {
                serializer = serializer ?? GetDefaultSerializer();
                var result = serializer.Serialize(value);
                container.Values[key] = compress ? result.Compress() : result;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}