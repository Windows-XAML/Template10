using System;
using Template10.Extensions;
using Template10.Services.Serialization;
using Windows.Storage;

namespace Template10.Services.Settings
{

    public abstract class SettingsService
    {
        public SettingsService()
            : this(ApplicationData.Current.LocalSettings, SerializationService.Default)
        {
            // empty
        }

        public SettingsService(ApplicationDataContainer container)
            : this(container, SerializationService.Default)
        {
            // empty
        }

        ApplicationDataContainer _container;
        ISerializationService _serializer;
        public SettingsService(ApplicationDataContainer container, ISerializationService serializer)
        {
            _container = container;
            _serializer = serializer;
        }

        protected bool TryRead<T>(string key, out T value)
        {
            if (!_container.Values.ContainsKey(key))
            {
                value = default(T);
                return false;
            }
            try
            {
                var result = _container.Values[key]?.ToString();
                result = CompressionHelper.Decompress(result);
                value = _serializer.Deserialize<T>(result);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        protected string ReadString(string key)
        {
            if (!_container.Values.ContainsKey(key))
            {
                return string.Empty;
            }
            var result = _container.Values[key]?.ToString();
            result = CompressionHelper.Decompress(result);
            return result;
        }

        protected bool TryWrite<T>(string key, T value)
        {
            try
            {
                var result = _serializer.Serialize(value);
                _container.Values[key] = result.Compress();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void WriteString(string key, string value)
        {
            _container.Values[key] = value.Compress();
        }
    }

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

        protected ISerializationAdapter DefaultSerializationStrategy { get; set; } = SerializationService.Default;

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
