using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultPersistenceFactory : IPersistedDictionaryFactory
    {
        public async Task<IPersistedDictionary> CreateAsync(string key)
        {
            return await FolderPersistenceStrategy.CreateAsync(FolderPersistenceLocations.Local, key: key);
        }

        public async Task<IPersistedDictionary> CreateAsync(string key, string container)
        {
            return await FolderPersistenceStrategy.CreateAsync(FolderPersistenceLocations.Local, container, key);
        }
    }
}
