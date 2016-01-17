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
        public object Serialize(object parameter)
        {
            return parameter?.ToString();
        }

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        public object Deserialize(object parameter)
        {
            return parameter;
        }
    }
}