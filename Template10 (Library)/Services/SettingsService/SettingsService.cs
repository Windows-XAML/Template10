using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        public static ISettingsService Local => _local ?? (_local = Create(SettingsStrategies.Local));

        private static ISettingsService _roaming;
        public static ISettingsService Roaming => _roaming ?? (_roaming = Create(SettingsStrategies.Roam));

        /// <summary>
        /// Creates an <c>ISettingsService</c> object targeting the requested (optional) <paramref name="folderName"/>
        /// in the <paramref name="strategy"/> container.
        /// </summary>
        /// <param name="strategy">Roaming or Local</param>
        /// <param name="folderName">Name of the settings folder to use</param>
        /// <param name="createFolderIfNotExists"><c>true</c> to create the folder if it isn't already there, false otherwise.</param>
        /// <returns></returns>
        public static ISettingsService Create(SettingsStrategies strategy, string folderName = null, bool createFolderIfNotExists = true)
        {
            ApplicationDataContainer rootContainer;
            switch (strategy)
            {
                case SettingsStrategies.Local:
                    rootContainer = ApplicationData.Current.LocalSettings;
                    break;
                case SettingsStrategies.Roam:
                    rootContainer = ApplicationData.Current.RoamingSettings;
                    break;
                default:
                    throw new ArgumentException($"Unsupported Settings Strategy: {strategy}", nameof(strategy));
            }

            ApplicationDataContainer targetContainer = rootContainer;
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                try
                {
                    targetContainer = rootContainer.CreateContainer(folderName, createFolderIfNotExists ? ApplicationDataCreateDisposition.Always : ApplicationDataCreateDisposition.Existing);
                }
                catch (Exception)
                {
                    throw new KeyNotFoundException($"No folder exists named '{folderName}'");
                }
            }

            return new SettingsService(targetContainer.Values);
        }

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
