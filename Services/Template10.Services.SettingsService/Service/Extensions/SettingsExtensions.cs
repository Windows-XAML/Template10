namespace Template10.Extensions
{
    using Windows.Storage;
    public static class SettingsExtensions
    {
        static Services.Serialization.ISerializationService GetDefaultSerializer()
        {
            var container = Services.DependencyInjection.DependencyServiceBase.Default;
            return container.Resolve<Services.Serialization.ISerializationService>();
        }

        public static bool TryRead<T>(this ApplicationDataContainer container, string key, out T value, bool decompress = true, Services.Serialization.ISerializationService serializer = null)
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

        public static bool TryWrite<T>(this ApplicationDataContainer container, string key, T value, bool compress = true, Services.Serialization.ISerializationService serializer = null)
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