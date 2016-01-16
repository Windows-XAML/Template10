using System;
using Newtonsoft.Json;

namespace Template10.Services.SerializationService
{
    public sealed class JsonSerializationService : ISerializationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationService"/> class.
        /// </summary>
        internal JsonSerializationService()
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
            else
            {
                var container = new Container
                {
                    Type = value.GetType().AssemblyQualifiedName,
                    Data = JsonConvert.SerializeObject(value, Formatting.None)
                };
                string result = JsonConvert.SerializeObject(container);
                return result;
            }
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public object Deserialize(object value)
        {
            string valueStr = value?.ToString();
            if (string.IsNullOrEmpty(valueStr))
            {
                return null;
            }
            else
            {
                Container container = JsonConvert.DeserializeObject<Container>(valueStr);
                Type type = Type.GetType(container.Type);
                object result = JsonConvert.DeserializeObject(container.Data, type);
                return result;
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