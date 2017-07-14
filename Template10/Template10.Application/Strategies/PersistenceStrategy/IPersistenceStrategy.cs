using System.Threading.Tasks;

namespace Template10.Strategies
{
    public interface IPersistenceStrategy
    {
        Task<string[]> AllKeysAsync();
        Task ClearAsync();
        Task<bool> ContainsKeyAsync(string key);
        Task<string> GetValueAsync(string key);
        Task SetValueAsync(string key, string value);
    }
}