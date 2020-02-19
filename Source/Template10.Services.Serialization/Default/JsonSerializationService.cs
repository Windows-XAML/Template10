using System.Text.Json;

namespace Template10.Services
{
    public class JsonSerializationService : ISerializationService
    {
        public object Deserialize(string parameter)
        {
            return JsonSerializer.Deserialize<object>(parameter);
        }

        public T Deserialize<T>(string parameter)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };
            return JsonSerializer.Deserialize<T>(parameter, options);
        }

        public string Serialize(object parameter)
        {
            return JsonSerializer.Serialize(parameter);
        }

        public bool TrySerialize(object parameter, out string result)
        {
            try
            {
                result = Serialize(parameter);
                return true;
            }
            catch 
            {
                result = default;
                return false;
            }
        }

        public bool TryDeserialize<T>(string parameter, out T result)
        {
            try
            {
                result = Deserialize<T>(parameter);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
