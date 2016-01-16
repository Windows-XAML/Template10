using System;

namespace Template10.Services.SerializationService
{
    public interface ISerializationService
    {
        /// <summary>
        /// Serializes the parameter.
        /// </summary>
        object Serialize(object parameter);

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        object Deserialize(object parameter);
    }
}