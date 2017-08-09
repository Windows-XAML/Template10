using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Common.PersistedDictionary
{
    public class PersistedDictionarySettingsAdapter : PersistedDictionaryBase
    {
        public PersistedDictionarySettingsAdapter(ApplicationDataContainer container) : base(container.Name)
            => this.container = container;

        private ApplicationDataContainer container;

        public async override Task ClearAsync()
            => container.Values.Clear();

        public override async Task<string[]> AllKeysAsync()
            => container.Values.Keys.ToArray();

        public async Task<bool> ContainsKeyAsync(string key)
            => container.Values.ContainsKey(key);

        public override async Task<string> GetStringAsync(string key)
            => container.Values[key]?.ToString() ?? string.Empty;

        public override async Task SetStringAsync(string key, string value)
        {
            container.Values[key] = value;
            base.RaiseMapChanged(key);
        }
    }
}
