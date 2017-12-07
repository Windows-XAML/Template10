using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Template10.Common
{
    public class TryResult<T>
    {
        public bool Success { get; set; }
        public T Value { get; set; }
    }

    public interface IPropertyBagEx : IPropertyBagExSimple
    {
        Task ClearAsync();
        Task<string[]> GetKeysAsync();
        Task<TryResult<T>> TryGetAsync<T>(string key);
        Task<TryResult<string>> TryGetStringAsync(string key);
        Task<bool> TrySetAsync<T>(string key, T value);
        Task<bool> TrySetStringAsync(string key, string value);
        IPropertyBagExPersistenceStrategy PersistenceStrategy { get; set; }
        IPropertyBagExSerializationStrategy SerializationStrategy { get; set; }
    }
}