using Template10.BCL;

namespace Template10.Services.Serialization
{
    public interface ISerializationService: IService
    {
        /// <summary>
        /// Serializes the parameter.
        /// </summary>
        string Serialize(object parameter);

        /// <summary>
        /// Attempts to serialize the parameter.
        /// </summary>
        bool TrySerialize<T>(T value, out string result);

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        object Deserialize(string parameter);

        /// <summary>
        /// Deserializes the parameter.
        /// </summary>
        T Deserialize<T>(string parameter);

        /// <summary>
        /// Attempts to deserialize the parameter.
        /// </summary>
        bool TryDeserialize<T>(string parameter, out T result);
    }
}