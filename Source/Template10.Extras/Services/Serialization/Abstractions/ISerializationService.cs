namespace Template10.Services.Serialization
{

    public interface ISerializationService
    {
        object Deserialize(string parameter);
        T Deserialize<T>(string parameter);
        string Serialize(object parameter);
        bool TrySerialize(object parameter, out string result);
        bool TryDeserialize<T>(string parameter, out T result);
    }
}