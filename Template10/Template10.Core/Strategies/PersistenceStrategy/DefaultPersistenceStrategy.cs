using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultPersistenceStrategy : PersistedDictionaryFolderAdapter
    {
        public static async Task<IPersistedDictionary> CreateAsync(string key)
        {
            return await CreateAsync(Settings.DefaultPersistenceStrategyFolderLocation, key: key);
        }

        public static async Task<IPersistedDictionary> CreateAsync(string key, string container)
        {
            return await CreateAsync(Settings.DefaultPersistenceStrategyFolderLocation, container, key);
        }

        private static async Task<IPersistedDictionary> CreateAsync(FolderPersistenceLocations location, string container = null, string key = null)
        {
            key = key ?? nameof(DefaultPersistenceStrategy);
            var root = default(StorageFolder);
            switch (location)
            {
                case FolderPersistenceLocations.Local:
                    root = ApplicationData.Current.LocalFolder;
                    break;
                case FolderPersistenceLocations.Roam:
                    root = ApplicationData.Current.RoamingFolder;
                    break;
            }
            root = await root.CreateFolderAsync("App-State", CreationCollisionOption.OpenIfExists);
            if (!string.IsNullOrEmpty(container))
            {
                root = await root.CreateFolderAsync(container, CreationCollisionOption.OpenIfExists);
            }
            var folder = await root.CreateFolderAsync(key, CreationCollisionOption.OpenIfExists);
            return new DefaultPersistenceStrategy(folder);
        }

        internal DefaultPersistenceStrategy(StorageFolder folder)
            : base(folder) { /* empty */ }
    }
}
