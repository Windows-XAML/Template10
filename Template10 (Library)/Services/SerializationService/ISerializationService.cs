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

        T Deserialize<T>(string parameter);
    }
}