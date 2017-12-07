using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Common
{
    public class MemoryPropertyBagPersistenceStrategy : IPropertyBagExPersistenceStrategy
    {
        internal MemoryPropertyBagPersistenceStrategy()
        {
            _dictionary = new Dictionary<string, object>();
        }

        Dictionary<string, object> _dictionary;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task ClearAsync() => _dictionary.Clear();

        public async Task<string[]> GetKeysAsync() => _dictionary.Keys.ToArray();

        public async Task<object> LoadAsync(string key) => _dictionary[key];

        public async Task SaveAsync(string key, object value) => _dictionary[key] = value;

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
