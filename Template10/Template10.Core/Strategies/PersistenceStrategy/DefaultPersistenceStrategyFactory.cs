using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultPersistenceStrategyFactory : IPersistedDictionaryFactory
    {
        public enum FolderPersistenceLocations { Local, Roam }
        FolderPersistenceLocations _location;

        public DefaultPersistenceStrategyFactory(FolderPersistenceLocations location)
        {
            _location = location;
        }

        public async Task<IPersistedDictionary> CreateAsync(string key)
        {
            return await CreateAsync(_location, key: key);
        }

        public async Task<IPersistedDictionary> CreateAsync(string key, string container)
        {
            return await CreateAsync(_location, container, key);
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
    }
}
