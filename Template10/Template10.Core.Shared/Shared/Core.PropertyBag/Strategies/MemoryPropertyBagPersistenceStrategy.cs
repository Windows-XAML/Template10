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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task ClearAsync() => dictionary.Clear();

        public async Task<string[]> GetKeysAsync() => dictionary.Keys.ToArray();

        public async Task<object> LoadAsync(string key) => dictionary[key];

        public async Task SaveAsync(string key, object value) => dictionary[key] = value;

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
