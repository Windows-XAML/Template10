using System;
using Newtonsoft.Json;
using Template10.Services.Serialization;

namespace Prism.Windows
{
    public class NewtonsoftSerializationService : ISerializationService
    {
        public NewtonsoftSerializationService()
        {
            Settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.Auto,
                // TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        /// <summary>
        /// JSON serializer settings.
        /// </summary>
        public JsonSerializerSettings Settings { get; }

        /// <summary>
        /// Serializes the value.
        /// </summary>
        public string Serialize(object value)
        {
            if (value == null)
                return null;

            if (value as string == string.Empty)
                return string.Empty;

            // Serialize to json
            return JsonConvert.SerializeObject(value);
        }

        public bool TrySerialize(object parameter, out string result)
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
        public object Deserialize(string value)
        {
            if (value == null)
                return null;

            if (value == string.Empty)
                return string.Empty;

            // Deserialize from json
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public T Deserialize<T>(string value)
        {
            if (value == null)
                return default(T);

            if (value == string.Empty)
                return default(T);

            // Deserialize from json
            return JsonConvert.DeserializeObject<T>(value);
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
    }
}
