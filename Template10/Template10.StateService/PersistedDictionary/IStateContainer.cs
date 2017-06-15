using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Services.StateService
{
    public interface IStateContainer
    {
        Task<string[]> AllKeysAsync();
        Task<bool> ContainsKeyAsync(string key);

        Task<T> GetValueAsync<T>(string key);
        Task<string> GetValueAsync(string key);

        Task SetValueAsync<T>(string key, T value);
        Task SetValueAsync(string key, string value);
    }
}