namespace Template10.Services.Serialization
{
    public class NullSerializationService : ISerializationService
    {
        public NullSerializationService()
        {
            throw new System.NotImplementedException("Custom implementation required");
        }

        public object Deserialize(string parameter)
        {
            throw new System.NotImplementedException();
        }

        public T Deserialize<T>(string parameter)
        {
            throw new System.NotImplementedException();
        }

        public string Serialize(object parameter)
        {
            throw new System.NotImplementedException();
        }

        public bool TryDeserialize<T>(string parameter, out T result)
        {
            throw new System.NotImplementedException();
        }

        public bool TrySerialize(object parameter, out string result)
        {
            throw new System.NotImplementedException();
        }
    }
}