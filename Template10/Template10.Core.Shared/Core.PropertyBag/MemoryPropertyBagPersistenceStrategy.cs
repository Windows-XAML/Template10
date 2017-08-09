using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Core
{
    public class MemoryPropertyBagPersistenceStrategy : IPropertyBagExPersistenceStrategy
    {
        Dictionary<string, object> dictionary;
        public MemoryPropertyBagPersistenceStrategy() => dictionary = new Dictionary<string, object>();

        public async Task ClearAsync() => dictionary.Clear();

        public async Task<string[]> GetKeysAsync() => dictionary.Keys.ToArray();

        public async Task<object> LoadAsync(string key) => dictionary[key];

        public async Task SaveAsync(string key, object value) => dictionary[key] = value;
    }
}
