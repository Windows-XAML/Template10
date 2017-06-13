using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.StateService
{
    public enum SettingsStateLocations { Local, Roam }

    public class SettingsStateContainer : StateContainerBase
    {
        public static async Task<SettingsStateContainer> CreateAsync(SettingsStateLocations location, string container = null, string key = null)
        {
            key = key ?? nameof(SettingsStateContainer);
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
            return new SettingsStateContainer(data);
        }

        private ApplicationDataContainer container;

        public async override Task ClearAsync()
            => container.Values.Clear();

        private SettingsStateContainer(ApplicationDataContainer container) : base(container.Name)
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
