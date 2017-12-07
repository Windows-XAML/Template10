using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Windows.Foundation.Collections;

namespace Template10.Common
{
    public class SettingsPropertyBagPersistenceStrategy : IPropertyBagExPersistenceStrategy
    {
        IPropertySet _settings;
        internal SettingsPropertyBagPersistenceStrategy(IPropertySet settings)
        {
            _settings = settings;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task ClearAsync() => _settings.Clear();

        public async Task<string[]> GetKeysAsync() => _settings.Keys.ToArray();

        public async Task<object> LoadAsync(string key) => _settings[key] as string;

        public async Task SaveAsync(string key, object value) => _settings[key] = value as string;

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
