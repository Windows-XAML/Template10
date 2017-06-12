using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.StateService
{
    public enum SettingsStateLocations { Local, Roam }

    public class SettingsStateContainer : StateContainerBase
    {
        public static async Task<SettingsStateContainer> CreateAsync(SettingsStateLocations location, string key = null)
        {
            key = key ?? nameof(SettingsStateContainer);
            var container = default(ApplicationDataContainer);
            switch (location)
            {
                case SettingsStateLocations.Local:
                    container = ApplicationData.Current.LocalSettings.CreateContainer(key, ApplicationDataCreateDisposition.Existing);
                    break;
                case SettingsStateLocations.Roam:
                    container = ApplicationData.Current.RoamingSettings.CreateContainer(key, ApplicationDataCreateDisposition.Existing);
                    break;
                default:
                    throw new NotSupportedException(location.ToString());
            }
            return new SettingsStateContainer(container);
        }

        private ApplicationDataContainer container;

        private SettingsStateContainer(ApplicationDataContainer container)
            => this.container = container;

        public override async Task<string[]> AllKeysAsync()
            => container.Values.Keys.ToArray();

        public async Task<bool> ContainsKeyAsync(string key)
            => container.Values.ContainsKey(key);

        public override async Task<string> GetValueAsync(string key)
            => container.Values[key]?.ToString() ?? string.Empty;

        public override async Task SetValueAsync(string key, string value)
            => container.Values[key] = value;
    }
}
