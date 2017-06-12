using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.StateService
{
    public abstract class StateContainerBase : IStateContainer
    {
        public abstract Task<string[]> AllKeysAsync();

        public virtual async Task<bool> ContainsKeyAsync(string key)
        {
            var keys = await AllKeysAsync();
            return keys.Contains(key);
        }

        public virtual async Task<T> GetValueAsync<T>(string key)
        {
            var setting = await GetValueAsync(key);
            var serializer = SerializationService.SerializationService.Json;
            var deserialized = serializer.Deserialize<T>(setting);
            return deserialized;
        }

        public abstract Task<string> GetValueAsync(string key);

        public async Task SetValueAsync<T>(string key, T value)
        {
            var serializer = SerializationService.SerializationService.Json;
            var serialized = serializer.Serialize(value);
            await SetValueAsync(key, serialized);
        }

        public abstract Task SetValueAsync(string key, string value);
    }
}
