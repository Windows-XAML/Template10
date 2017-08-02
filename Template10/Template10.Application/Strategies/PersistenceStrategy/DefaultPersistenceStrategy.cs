using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultPersistenceStrategy : PersistedDictionaryFolderAdapter
    {
        internal DefaultPersistenceStrategy(StorageFolder folder)
            : base(folder) { /* empty */ }
    }
}
