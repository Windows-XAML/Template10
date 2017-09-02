using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Windows.Foundation.Collections;

namespace Template10.Core
{
    public class SettingsPropertyBagPersistenceStrategy : IPropertyBagExPersistenceStrategy
    {
        IPropertySet settings;
        public SettingsPropertyBagPersistenceStrategy(IPropertySet settings) => this.settings = settings;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task ClearAsync() => settings.Clear();

        public async Task<string[]> GetKeysAsync() => settings.Keys.ToArray();

        public async Task<object> LoadAsync(string key) => settings[key] as string;

        public async Task SaveAsync(string key, object value) => settings[key] = value as string;

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
