using System;

namespace Template10.Services.SerializationService
{
    public static class SerializationService
    {
        private static volatile ISerializationService instance = new JsonSerializationService();
        private static volatile Tuple<object, object> lastCache = new Tuple<object, object>(null, null);

        /// <summary>
        /// Gets or sets the instance that should be used to serialize/deserialize.
        /// </summary>
        public static ISerializationService Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                lastCache = new Tuple<object, object>(null, null);
            }
        }


        /// <summary>
        /// Serializes the value.
        /// </summary>
        public static object Serialize(object value)
        {
            var lastCacheValue = lastCache;
            if (ReferenceEquals(lastCacheValue.Item1, value))
            {
                return lastCacheValue.Item2;
            }
            else
            {
                var result = instance.Serialize(value);
                lastCache = new Tuple<object, object>(value, result);
                return result;
            }
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public static object Deserialize(object value)
        {
            var lastCacheValue = lastCache;
            if (ReferenceEquals(lastCacheValue.Item2, value))
            {
                return lastCacheValue.Item1;
            }
            else
            {
                var result = instance.Deserialize(value);
                lastCache = new Tuple<object, object>(result, value);
                return result;
            }
        }
    }
}