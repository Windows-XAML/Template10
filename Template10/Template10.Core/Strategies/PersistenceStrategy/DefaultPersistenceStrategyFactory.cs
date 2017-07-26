using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Storage;

namespace Template10.Strategies
{
    public enum FolderPersistenceLocations { Local, Roam }

    // this factory is used in Navigation

    public class DefaultPersistenceStrategyFactory : IPersistedDictionaryFactory
    {
        public Task<IPersistedDictionary> CreateAsync(string key)
        {
            return DefaultPersistenceStrategy.CreateAsync(key);
        }

        public Task<IPersistedDictionary> CreateAsync(string key, string container)
        {
            return DefaultPersistenceStrategy.CreateAsync(key, container);
        }
    }
}
