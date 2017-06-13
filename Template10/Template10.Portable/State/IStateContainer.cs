using System.Threading.Tasks;

namespace Template10.Portable.State
{
    public interface IPersistedStateContainer
    {
        string Name { get; }

        Task<string[]> AllKeysAsync();
        Task<bool> ContainsKeyAsync(string key);

        Task<T> GetValueAsync<T>(string key);
        Task<string> GetValueAsync(string key);

        Task<(bool Success, object Value)> TryGetValueAsync(string key);
        Task<(bool Success, T Value)> TryGetValueAsync<T>(string key);

        Task SetValueAsync<T>(string key, T value);
        Task SetValueAsync(string key, string value);

        Task ClearAsync();
    }
}