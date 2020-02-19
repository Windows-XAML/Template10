using System;

namespace Template10.Services.Settings
{
    public class SettingsHelper : ISettingsHelper
    {
        private readonly ISerializationService _serializationService;
        private readonly ICompressionService _compressionService;
        private readonly ISettingsAdapter _adapter;

        //public SettingsHelper(ICompressionService compressionService)
        //    : this(new LocalSettingsAdapter(), compressionService)
        //{
        //    // empty
        //}

        public SettingsHelper(ISettingsAdapter adapter, ICompressionService compressionService)
        {
            _adapter = adapter;
            _compressionService = compressionService;
            _serializationService = adapter.SerializationService;
        }

        public bool EnableCompression { get; set; } = false;

        public T Read<T>(string key)
        {
            var result = _adapter.ReadString(key);
            if (EnableCompression)
            {
                result = _compressionService.Unzip(result);
            }
            return _serializationService.Deserialize<T>(result);
        }

        public T SafeRead<T>(string key, T otherwise)
        {
            if (TryRead<T>(key, out var value))
            {
                return value;
            }
            else
            {
                return otherwise;
            }
        }


        public T SafeReadEnum<T>(string key, T otherwise) where T : struct
        {
            if (TryReadEnum<T>(key, out var value))
            {
                return value;
            }
            else
            {
                return otherwise;
            }
        }

        public bool TryRead<T>(string key, out T value)
        {
            try
            {
                value = Read<T>(key);
                return true;
            }
            catch (System.Exception)
            {
                value = default(T);
                return false;
            }
        }

        public bool TryReadEnum<T>(string key, out T value) where T : struct
        {
            try
            {
                if (TryReadString(key, out var setting))
                {
                    if (Enum.TryParse<T>(setting, out var result))
                    {
                        value = result;
                        return true;
                    }
                }
                value = default(T);
                return false;
            }
            catch (System.Exception)
            {
                value = default(T);
                return false;
            }
        }

        public string ReadString(string key)
        {
            var result = _adapter.ReadString(key);
            if (EnableCompression)
            {
                return _compressionService.Unzip(result);
            }
            else
            {
                return result;
            }
        }

        public bool TryReadString(string key, out string value)
        {
            try
            {
                value = ReadString(key);
                return true;
            }
            catch (System.Exception)
            {
                value = string.Empty;
                return false;
            }
        }

        public void Write<T>(string key, T value)
        {
            var result = _serializationService.Serialize(value);
            if (EnableCompression)
            {
                result = _compressionService.Zip(result);
            }
            _adapter.WriteString(key, result);
        }

        public bool TryWrite<T>(string key, T value)
        {
            try
            {
                Write(key, value);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public void WriteString(string key, string value)
        {
            if (EnableCompression)
            {
                value = _compressionService.Zip(value);
            }
            _adapter.WriteString(key, value);
        }

        public void WriteEnum<T>(string key, T value) where T : struct
        {
            var newvalue = value.ToString();
            if (EnableCompression)
            {
                newvalue = _compressionService.Zip(newvalue);
            }
            _adapter.WriteString(key, newvalue);
        }

        public bool TryWriteString(string key, string value)
        {
            try
            {
                WriteString(key, value);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
