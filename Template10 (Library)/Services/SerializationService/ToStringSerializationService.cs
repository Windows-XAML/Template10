namespace Template10.Services.SerializationService
{
    internal sealed class ToStringSerializationService : ISerializationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToStringSerializationService"/> class.
        /// </summary>
        internal ToStringSerializationService()
        {
        }

        /// <summary>
        /// Serializes the parameter.
        /// </summary>
        public string Serialize(object parameter)
        {
            return parameter?.ToString();
        }

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        public T Deserialize<T>(string value) =>
           (T)Deserialize(value);

        public object Deserialize(string parameter)
        {
            return parameter;
        }
    }
}