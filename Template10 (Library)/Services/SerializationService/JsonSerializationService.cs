using System;
using Newtonsoft.Json;

namespace Template10.Services.SerializationService
{
    public sealed class JsonSerializationService : ISerializationService
    {
        private volatile Tuple<object, string> lastCache = new Tuple<object, string>(null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationService"/> class.
        /// </summary>
        internal JsonSerializationService()
        {
        }

        /// <summary>
        /// Serializes the value.
        /// </summary>
        public string Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }
            string valueStr = value as string;
            if (valueStr == string.Empty)
            {
                return valueStr;
            }

            // Check cache
            var lastCacheValue = lastCache;
            if (ReferenceEquals(lastCacheValue.Item1, value))
            {
                return lastCacheValue.Item2;
            }

            // Serialize to json
            var container = new Container
            {
                Type = value.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(value, Formatting.None)
            };
            var result = JsonConvert.SerializeObject(container);

            // Update the cache
            lastCache = new Tuple<object, string>(value, result);
            return result;
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public object Deserialize(string value)
        {
            if (value == null)
            {
                return null;
            }
            if (value == string.Empty)
            {
                return string.Empty;
            }

            // Check cache
            var lastCacheValue = lastCache;
            if (string.Equals(lastCacheValue.Item2, value))
            {
                return lastCacheValue.Item1;
            }

            // Deserialize from json
            Container container = JsonConvert.DeserializeObject<Container>(value);
            Type type = Type.GetType(container.Type);
            object result = JsonConvert.DeserializeObject(container.Data, type);

            // Update the cache
            lastCache = new Tuple<object, string>(result, value);
            return result;
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