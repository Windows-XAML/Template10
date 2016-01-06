using Newtonsoft.Json;
using System;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.SettingsService
{
    public interface ISettingsService
    {
        IPropertyMapping Converters { get; set; }
        bool Exists(string key);
        T Read<T>(string key, T fallback);
        void Remove(string key);
        void Write<T>(string key, T value);
    }

    // https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public class SettingsService : ISettingsService
    {
        private static ISettingsService _local;
        public static ISettingsService Local => _local ?? (_local = new SettingsService(ApplicationData.Current.LocalSettings.Values));

        private static ISettingsService _roaming;
        public static ISettingsService Roaming => _roaming ?? (_roaming = new SettingsService(ApplicationData.Current.RoamingSettings.Values));

        protected IPropertySet Values { get; private set; }

        public IPropertyMapping Converters { get; set; } = new JsonMapping();

        private SettingsService(IPropertySet values) { Values = values; }

        public bool Exists(string key) => Values.ContainsKey(key);

        public void Remove(string key)
        {
            if (Values.ContainsKey(key))
                Values.Remove(key);
        }

        public void Write<T>(string key, T value)
        {
            var type = typeof(T);
            var converter = Converters.GetConverter(type);
            var container = new ApplicationDataCompositeValue();
            var converted = converter.ToStore(value, type);
            container["Value"] = converted;
            Values[key] = container;
        }

        public T Read<T>(string key, T fallback)
        {
            try
            {
                if (Values.ContainsKey(key))
                {
                    var type = typeof(T);
                    var converter = Converters.GetConverter(type);
                    var container = Values[key] as ApplicationDataCompositeValue;
                    if (container.ContainsKey("Value"))
                    {
                        var value = container["Value"] as string;
                        var converted = (T)converter.FromStore(value, type);
                        return converted;
                    }
                }
                return fallback;
            }
            catch
            {
                return fallback;
            }
        }
    }

    public interface IStoreConverter
    {
        string ToStore(object value, Type type);
        object FromStore(string value, Type type);
    }

    public interface IPropertyMapping
    {
        IStoreConverter GetConverter(Type type);
    }

    public class JsonMapping : IPropertyMapping
    {
        protected IStoreConverter jsonConverter = new JsonConverter();
        public IStoreConverter GetConverter(Type type) => jsonConverter;
    }

    public class JsonConverter : IStoreConverter
    {
        public object FromStore(string value, Type type) => JsonConvert.DeserializeObject(value, type);
        public string ToStore(object value, Type type) => JsonConvert.SerializeObject(value);
    }
}
