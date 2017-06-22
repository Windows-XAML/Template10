using System;
using System.Threading.Tasks;

namespace Template10.Common.PersistedDictionary
{
    public static class PersistedDictionaryHelper
    {
        public static async Task<IPersistedDictionary> GetAsync(string key, string container = null, PersistedDictionaryTypes type = PersistedDictionaryTypes.FileSystem)
        {
            switch (type)
            {
                case PersistedDictionaryTypes.Settings: return await Strategies.SettingPersistedDictionaryStrategy
                        .CreateAsync(Strategies.SettingsStateLocations.Local, container, key);

                case PersistedDictionaryTypes.FileSystem: return await Strategies.FolderPersistedDictionaryStrategy
                        .CreateAsync(Strategies.FolderPersistedDictionaryLocations.Local, container, key);

                default: throw new NotSupportedException(type.ToString());
            }
        }
    }
}
