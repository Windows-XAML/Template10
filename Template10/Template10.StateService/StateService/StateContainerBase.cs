using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Portable.State;

namespace Template10.Services.StateService
{
    public abstract class StateContainerBase : IPersistedStateContainer
    {
        public string Name { get; }

        public StateContainerBase(string name)
        {
            Name = name;
        }

        public abstract Task<string[]> AllKeysAsync();

        public abstract Task ClearAsync();

        public virtual async Task<bool> ContainsKeyAsync(string key)
        {
            var keys = await AllKeysAsync();
            return keys.Contains(key);
        }

        public virtual async Task<T> GetValueAsync<T>(string key)
        {
            var setting = await GetValueAsync(key);
            var serializer = StateService.DefaultSerializationStrategy;
            var deserialized = serializer.Deserialize<T>(setting);
            return deserialized;
        }

        public abstract Task<string> GetValueAsync(string key);

        public async Task SetValueAsync<T>(string key, T value)
        {
            var serializer = StateService.DefaultSerializationStrategy;
            var serialized = serializer.Serialize(value);
            await SetValueAsync(key, serialized);
        }

        public abstract Task SetValueAsync(string key, string value);

        public async Task<(bool Success, object Value)> TryGetValueAsync(string key)
        {
            try
            {
                var value = await GetValueAsync(key);
                return (true, value);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<(bool Success, T Value)> TryGetValueAsync<T>(string key)
        {
            try
            {
                var value = await GetValueAsync<T>(key);
                return (true, value);
            }
            catch
            {
                return (false, default(T));
            }
        }
    }
}
