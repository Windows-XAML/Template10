using System.ComponentModel;
using System.Threading.Tasks;

namespace Template10.Portable.PersistedDictionary
{
    public interface IPersistedDictionary
    {
        event PropertyChangedEventHandler MapChanged;

        string Name { get; }

        Task<string[]> AllKeysAsync();
        Task<bool> ContainsKeyAsync(string key);

        Task<T> GetValueAsync<T>(string key);
        Task<string> GetValueAsync(string key);

        Task<(bool Success, string Value)> TryGetValueAsync(string key);
        Task<(bool Success, T Value)> TryGetValueAsync<T>(string key);

        Task SetValueAsync<T>(string key, T value);
        Task SetValueAsync(string key, string value);

        Task ClearAsync();
    }
}