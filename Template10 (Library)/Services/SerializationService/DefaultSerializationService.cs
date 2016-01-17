namespace Template10.Services.SerializationService
{
    internal sealed class DefaultSerializationService : ISerializationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSerializationService"/> class.
        /// </summary>
        internal DefaultSerializationService()
        {
        }

        /// <summary>
        /// Serializes the parameter.
        /// </summary>
        public object Serialize(object parameter)
        {
            return parameter;
        }

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        public object Deserialize(object parameter)
        {
            return parameter;
        }

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        public T Deserialize<T>(object value)
        {
            if (value != null)
            {
                return (T)value;
            }
            return default(T);
        }
    }
}