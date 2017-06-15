using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Common.PersistedDictionary
{
    public enum SettingsStateLocations { Local, Roam }

    public class SettingPersistedDictionaryStrategy : PersistedDictionaryBase
    {
        public static async Task<SettingPersistedDictionaryStrategy> CreateAsync(SettingsStateLocations location, string container = null, string key = null)
        {
            key = key ?? nameof(SettingPersistedDictionaryStrategy);
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
            return new SettingPersistedDictionaryStrategy(data);
        }

        private ApplicationDataContainer container;

        public async override Task ClearAsync()
            => container.Values.Clear();

        private SettingPersistedDictionaryStrategy(ApplicationDataContainer container) : base(container.Name)
            => this.container = container;

        public override async Task<string[]> AllKeysAsync()
            => container.Values.Keys.ToArray();

        public async Task<bool> ContainsKeyAsync(string key)
            => container.Values.ContainsKey(key);

        public override async Task<string> GetValueAsync(string key)
            => container.Values[key]?.ToString() ?? string.Empty;

        public override async Task SetValueAsync(string key, string value)
        {
            container.Values[key] = value;
            base.RaiseMapChanged(key);
        }
    }
}
