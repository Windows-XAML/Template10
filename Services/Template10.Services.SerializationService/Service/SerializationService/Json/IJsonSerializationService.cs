using Newtonsoft.Json;

namespace Template10.Services.Serialization
{
    public interface IJsonSerializationService : ISerializationService
    {
        JsonSerializerSettings Settings { get; }
    }
}