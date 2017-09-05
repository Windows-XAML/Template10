using System;
using Template10.Extensions;
using Template10.Services.Serialization;
using Windows.Storage;

namespace Template10.Services.Settings
{
    public abstract class SettingsServiceBase
    {
        ApplicationDataContainer _DataContainer;
        ISerializationService _SerializationService;
        public SettingsServiceBase(ApplicationDataContainer data, ISerializationService serializer = null)
        {
            _DataContainer = data;
            _SerializationService = serializer = new JsonSerializationService();
        }

        protected T Read<T>(string key, T otherwise) => TryRead<T>(key, out var value) ? value : otherwise;

        protected bool TryRead<T>(string key, out T value)
        {
            var success = _DataContainer.TryRead<T>(key, out var result, true, _SerializationService);
            value = result;
            return success;
        }

        protected string ReadString(string key)
        {
            if (!_DataContainer.Values.ContainsKey(key))
            {
                return string.Empty;
            }
            var result = _DataContainer.Values[key]?.ToString();
            result = result.Decompress();
            return result;
        }

        protected void Write<T>(string key, T value) => _DataContainer.TryWrite(key, value, true, _SerializationService);

        protected bool TryWrite<T>(string key, T value) => _DataContainer.TryWrite<T>(key, value, true, _SerializationService);

        protected void WriteString(string key, string value)
        {
            _DataContainer.Values[key] = value.Compress();
        }
    }
}
