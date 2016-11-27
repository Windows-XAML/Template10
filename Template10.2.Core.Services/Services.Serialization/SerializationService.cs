using System;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using Template10.Services.Logging;
using System.Reflection;

namespace Template10.Services.Serialization
{
    public sealed class SerializationService : ISerializationService
    {
        public static SerializationService Instance { get; } = new SerializationService();

        private SerializationService()
        {
            // private constructor
            settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        private readonly JsonSerializerSettings settings;

        #region Debug

        static ILoggingService _logginService = new LoggingService();
        static void DebugWrite(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null) =>
            _logginService.WriteLine(text, severity, caller: $"{nameof(SerializationService)}.{caller}");

        #endregion

        /// <summary>
        /// Serializes the value.
        /// </summary>
        public string Serialize(object value)
        {
            DebugWrite($"Value {value}");

            if (value == null)
                return null;

            if (value as string == string.Empty)
                return string.Empty;

            // Serialize to json
            var container = new Container
            {
                Type = value.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(value, Formatting.None, settings)
            };
            return JsonConvert.SerializeObject(container);
        }

        public bool TrySerialize<T>(T value, out string result)
        {
            try
            {
                result = this.Serialize(value);
                return true;
            }
            catch
            {
                result = string.Empty;
                return false;
            }
        }

        public bool CanSerialize<T>(T value)
        {
            if (value == null)
            {
                return true;
            }
            if (value?.GetType().GetTypeInfo().IsSerializable ?? true)
            {
                return true;
            }
            string result;
            return TrySerialize(value, out result);
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public object Deserialize(string value)
        {
            DebugWrite($"Value {value}");

            if (value == null)
                return null;

            if (value == string.Empty)
                return string.Empty;

            // Deserialize from json
            var container = JsonConvert.DeserializeObject<Container>(value);
            var type = Type.GetType(container.Type);
            return JsonConvert.DeserializeObject(container.Data, type, settings);
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public T Deserialize<T>(string value)
        {
            object result = this.Deserialize(value);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        /// <summary>
        /// Attempts to deserialize the value by absorbing the InvalidCastException that may occur.
        /// </summary>
        /// <returns>
        /// True if deserialization succeeded with non-null result, otherwise False.
        /// </returns>
        /// <remarks>
        /// On success (or return True) deserialized result is copied to the 'out' variable.
        /// On fail (or return False) default(T) is copied to the 'out' variable.
        /// </remarks>
        public bool TryDeserialize<T>(string value, out T result)
        {
            try
            {
                object r = this.Deserialize(value);
                if (r == null)
                {
                    result = default(T);
                    return false;
                }
                result = (T)r;
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        #region Internal Container Class

        sealed class Container
        {
            public string Type { get; set; }
            public string Data { get; set; }
        }

        #endregion
    }
}