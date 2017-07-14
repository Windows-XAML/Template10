using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.SerializationService;

namespace Template10.Common.PersistedDictionary
{
    public abstract class PersistedDictionaryBase : IPersistedDictionary
    {
        protected ISerializationService _serializer = SerializationHelper.Json;

        public event PropertyChangedEventHandler MapChanged;
        protected void RaiseMapChanged(string propertyName)
            => MapChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Name { get; }

        public PersistedDictionaryBase(string name, ISerializationService customSerializer)
            : this(name) => _serializer = customSerializer;

        public PersistedDictionaryBase(string name)
            => Name = name;

        public abstract Task<string[]> AllKeysAsync();

        public abstract Task ClearAsync();

        public virtual async Task<bool> ContainsKeyAsync(string key)
            => (await AllKeysAsync()).Contains(key);

        public virtual async Task<T> GetValueAsync<T>(string key)
            => _serializer.Deserialize<T>(await GetValueAsync(key));

        public abstract Task<string> GetValueAsync(string key);

        public async Task SetValueAsync<T>(string key, T value)
            => await SetValueAsync(key, _serializer.Serialize(value));

        public abstract Task SetValueAsync(string key, string value);

        public async Task<(bool Success, string Value)> TryGetValueAsync(string key)
        {
            try { return (true, await GetValueAsync(key)); }
            catch { return (false, null); }
        }

        public async Task<(bool Success, T Value)> TryGetValueAsync<T>(string key)
        {
            try { return (true, await GetValueAsync<T>(key)); }
            catch { return (false, default(T)); }
        }
    }
}
