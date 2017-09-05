using System.Threading.Tasks;

namespace Template10.Common
{
    public interface IPropertyBagExPersistenceStrategy
    {
        Task<string[]> GetKeysAsync();
        Task ClearAsync(); 
        Task SaveAsync(string key, object value);
        Task<object> LoadAsync(string key);
    }
}