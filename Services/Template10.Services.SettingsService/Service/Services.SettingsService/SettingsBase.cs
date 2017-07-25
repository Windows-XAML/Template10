using System;
using Template10.Services.SerializationService;
using Windows.Storage;

namespace Template10.Services.SettingsService.Services.SettingsService
{
    public abstract class SettingsServiceBase
    {
        ApplicationDataContainer _container;

        public SettingsServiceBase()
            : this(ApplicationData.Current.LocalSettings, nameof(SettingsServiceBase))
        {
            // empty
        }

        public SettingsServiceBase(ApplicationDataContainer container)
            : this(container, nameof(SettingsServiceBase))
        {
            // empty
        }

        public SettingsServiceBase(ApplicationDataContainer container, string name)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (container.Containers.ContainsKey(name))
            {
                _container = container.Containers[name];
            }
            else
            {
                _container = container.CreateContainer(name, ApplicationDataCreateDisposition.Always); ;
            }
        }

        protected ISerializationService DefaultSerializationStrategy { get; set; } = SerializationHelper.Json;

        protected T Read<T>(string key, T otherwise) => TryRead<T>(key, out var value) ? value : otherwise;

        protected bool TryRead<T>(string key, out T setting)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                setting = default(T);
                return true;
            }

            try
            {
                var value = _container.Values[key] as string;
                value = CompressionHelper.Decompress(value);
                setting = DefaultSerializationStrategy.Deserialize<T>(value);
                return true;
            }
            catch (Exception)
            {
                setting = default(T);
                return false;
            }
        }

        protected void Write<T>(string key, T setting)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            var value = DefaultSerializationStrategy.Serialize(setting);
            value = CompressionHelper.Compress(value);
            _container.Values[key] = value;
        }
    }
}
