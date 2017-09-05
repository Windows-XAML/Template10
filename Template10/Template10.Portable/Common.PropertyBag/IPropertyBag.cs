using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Template10.Common
{

    public interface IPropertyBagEx : IPropertyBagExSimple
    {
        Task ClearAsync();
        Task<string[]> GetKeysAsync();
        Task<(bool Success, T Value)> TryGetAsync<T>(string key);
        Task<(bool Success, string Value)> TryGetStringAsync(string key);
        Task<bool> TrySetAsync<T>(string key, T value);
        Task<bool> TrySetStringAsync(string key, string value);
        IPropertyBagExPersistenceStrategy PersistenceStrategy { get; set; }
        IPropertyBagExSerializationStrategy SerializationStrategy { get; set; }
    }
}