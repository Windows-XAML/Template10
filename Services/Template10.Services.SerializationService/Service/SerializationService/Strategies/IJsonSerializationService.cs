using Newtonsoft.Json;

namespace Template10.Services.SerializationService.Strategies
{
    public interface IJsonSerializationService : ISerializationService
    {
        JsonSerializerSettings Settings { get; }
    }
}