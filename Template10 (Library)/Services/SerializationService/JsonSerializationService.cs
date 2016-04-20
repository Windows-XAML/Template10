using System;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Template10.Services.SerializationService
{
    using System.Diagnostics;
    using System.Runtime.Serialization.Formatters;

    public sealed class JsonSerializationService : ISerializationService
    {
        private readonly JsonSerializerSettings settings;

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(JsonSerializationService)}.{caller}");

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationService"/> class.
        /// </summary>
        internal JsonSerializationService()
        {
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

        /// <summary>
        /// JSON serializer settings.
        /// </summary>
        public JsonSerializerSettings Settings => settings;

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

        #region Internal Container Class

        sealed class Container
        {
            public string Type { get; set; }
            public string Data { get; set; }
        }

        #endregion
    }
}