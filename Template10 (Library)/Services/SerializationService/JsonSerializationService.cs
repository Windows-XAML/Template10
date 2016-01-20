using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Template10.Services.SerializationService
{
    public sealed class JsonSerializationService : ISerializationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationService"/> class.
        /// </summary>
        public JsonSerializationService()
        {
        }

        /// <summary>
        /// Serializes the value.
        /// </summary>
        public object Serialize(object value)
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

            var container = new Container
            {
                Type = value.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(value, Formatting.None)
            };
            return JsonConvert.SerializeObject(container);
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public object Deserialize(object value)
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

            Container container = JsonConvert.DeserializeObject<Container>(valueStr);
            Type type = Type.GetType(container.Type);
            object result = JsonConvert.DeserializeObject(container.Data, type);
            return result;
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public T Deserialize<T>(object value)
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