using System;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultPersistedDictionary : PersistedDictionaryFolderAdapter
    {
        internal DefaultPersistedDictionary(StorageFolder folder)
            : base(folder) { /* empty */ }
    }
}
