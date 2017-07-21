using System.ComponentModel;
using System.Threading.Tasks;

namespace Template10.Portable.Common
{
    public interface IPropertyBagAsync
    {
        event PropertyChangedEventHandler MapChanged;

        Task<string[]> AllKeysAsync();
        Task<bool> ContainsKeyAsync(string key);

        Task<T> GetValueAsync<T>(string key);

        Task SetValueAsync<T>(string key, T value);

        Task<string> GetStringAsync(string key);

        Task SetStringAsync(string key, string value);

        Task<(bool Success, string Value)> TryGetStringAsync(string key);
        Task<(bool Success, T Value)> TryGetValueAsync<T>(string key);

        Task ClearAsync();
    }
}