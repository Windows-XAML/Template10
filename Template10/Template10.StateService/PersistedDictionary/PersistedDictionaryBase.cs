using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Template10.Common.PersistedDictionary
{
    public abstract class PersistedDictionaryBase : IPersistedDictionary
    {
        public event PropertyChangedEventHandler MapChanged;
        protected void RaiseMapChanged(string propertyName) 
            => MapChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        public string Name { get; }

        public PersistedDictionaryBase(string name)
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
            var serializer = Settings.SerializationStrategy;
            var deserialized = serializer.Deserialize<T>(setting);
            return deserialized;
        }

        public abstract Task<string> GetValueAsync(string key);

        public async Task SetValueAsync<T>(string key, T value)
        {
            var serializer = Settings.SerializationStrategy;
            var serialized = serializer.Serialize(value);
            await SetValueAsync(key, serialized);
        }

        public abstract Task SetValueAsync(string key, string value);

        public async Task<(bool Success, string Value)> TryGetValueAsync(string key)
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
