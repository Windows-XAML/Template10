using System;
using System.Threading.Tasks;
using Template10.Portable.PersistedDictionary;

namespace Template10.Common.PersistedDictionary
{
    public enum PersistedDictionaryTypes { Settings, FileSystem }

    public static class Settings
    {
        public static Services.SerializationService.ISerializationService SerializationStrategy { get; set; }
            = Services.SerializationService.SerializationService.Json;
    }

    public static class PersistedDictionaryHelper
    {
        public static async Task<IPersistedDictionary> GetAsync(string key, string container = null, PersistedDictionaryTypes type = PersistedDictionaryTypes.FileSystem)
        {
            switch (type)
            {
                case PersistedDictionaryTypes.Settings: return await SettingPersistedDictionaryStrategy.CreateAsync(SettingsStateLocations.Local, container, key);
                case PersistedDictionaryTypes.FileSystem: return await FolderPersistedDictionaryStrategy.CreateAsync(FolderPersistedDictionaryLocations.Local, container, key);
                default: throw new NotSupportedException(type.ToString());
            }
        }
    }
}
