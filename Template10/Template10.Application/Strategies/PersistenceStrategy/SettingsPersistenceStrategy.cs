using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Strategies
{
    public class SettingsPersistenceStrategy : PersistedDictionaryBase, IPersistenceStrategy
    {
        public static async Task<IPersistedDictionary> CreateAsync(SettingsStateLocations location, string container = null, string key = null)
        {
            key = key ?? nameof(SettingsPersistenceStrategy);
            var data = default(ApplicationDataContainer);
            switch (location)
            {
                case SettingsStateLocations.Local:
                    data = ApplicationData.Current.LocalSettings.CreateContainer(key, ApplicationDataCreateDisposition.Existing);
                    break;
                case SettingsStateLocations.Roam:
                    data = ApplicationData.Current.RoamingSettings.CreateContainer(key, ApplicationDataCreateDisposition.Existing);
                    break;
                default:
                    throw new NotSupportedException(location.ToString());
            }
            if (!string.IsNullOrEmpty(container))
            {
                data = data.CreateContainer(container, ApplicationDataCreateDisposition.Existing);
            }
            return new SettingsPersistenceStrategy(data);
        }

        private ApplicationDataContainer container;

        public async override Task ClearAsync()
            => container.Values.Clear();

        private SettingsPersistenceStrategy(ApplicationDataContainer container) : base(container.Name)
            => this.container = container;

        public override async Task<string[]> AllKeysAsync()
            => container.Values.Keys.ToArray();

        public async Task<bool> ContainsKeyAsync(string key)
            => container.Values.ContainsKey(key);

        protected override async Task<string> GetStringAsync(string key)
            => container.Values[key]?.ToString() ?? string.Empty;

        protected override async Task SetStringAsync(string key, string value)
        {
            container.Values[key] = value;
            base.RaiseMapChanged(key);
        }
    }
}
