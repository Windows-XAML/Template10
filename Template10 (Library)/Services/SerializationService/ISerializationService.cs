using System;

namespace Template10.Services.SerializationService
{
    public interface ISerializationService
    {
        /// <summary>
        /// Serializes the parameter.
        /// </summary>
        string Serialize(object parameter);

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        object Deserialize(string parameter);

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        T Deserialize<T>(string parameter);
    }
}