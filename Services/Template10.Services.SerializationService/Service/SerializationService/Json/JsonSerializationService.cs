using System;

namespace Template10.Services.Serialization
{
    public class JsonSerializationService : SerializationService, IJsonSerializationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationAdapter"/> class.
        /// </summary>
        public JsonSerializationService()
        {
            Settings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.None,
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                //TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Auto,
                ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        /// <summary>
        /// JSON serializer settings.
        /// </summary>
        public Newtonsoft.Json.JsonSerializerSettings Settings { get; }

        /// <summary>
        /// Serializes the value.
        /// </summary>
        public override string Serialize(object value)
        {
            if (value == null)
                return null;

            if (value as string == string.Empty)
                return string.Empty;

            // Serialize to json
            var container = new Container
            {
                Type = value.GetType().AssemblyQualifiedName,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, Settings)
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(container);
        }

        public override bool TrySerialize(object parameter, out string result)
        {
            try
            {
                result = Serialize(parameter);
                return true;
            }
            catch (Exception)
            {
                result = default(string);
                return false;
            }
        }


        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public override object Deserialize(string value)
        {
            if (value == null)
                return null;

            if (value == string.Empty)
                return string.Empty;

            // Deserialize from json
            var container = Newtonsoft.Json.JsonConvert.DeserializeObject<Container>(value);
            var type = Type.GetType(container.Type);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(container.Data, type, Settings);
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public override T Deserialize<T>(string value)
        {
            if (value == null)
                return default(T);

            if (value == string.Empty)
                return default(T);

            // Deserialize from json
            var container = Newtonsoft.Json.JsonConvert.DeserializeObject<Container>(value);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(container.Data);
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
        public override bool TryDeserialize<T>(string value, out T result)
        {
            try
            {
                var r = this.Deserialize<T>(value);
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