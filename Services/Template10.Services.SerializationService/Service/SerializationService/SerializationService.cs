namespace Template10.Services.Serialization
{

    public abstract class SerializationService : ISerializationService
    {
        public abstract object Deserialize(string parameter);
        public abstract T Deserialize<T>(string parameter);
        public abstract bool TrySerialize(object parameter, out string result);
        public abstract string Serialize(object parameter);
        public abstract bool TryDeserialize<T>(string parameter, out T result);
    }
}