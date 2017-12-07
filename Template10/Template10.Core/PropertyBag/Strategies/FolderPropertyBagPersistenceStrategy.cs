using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Windows.Storage;

namespace Template10.Common
{
    public class FolderPropertyBagPersistenceStrategy : IPropertyBagExPersistenceStrategy
    {
        internal FolderPropertyBagPersistenceStrategy()
        {
            // empty
        }

        private StorageFolder _folder;

        public FolderPropertyBagPersistenceStrategy(StorageFolder folder) => this._folder = folder;

        public async Task ClearAsync()
        {
            var name = _folder.Name;
            var parent = await _folder.GetParentAsync();
            try
            {
                await _folder.DeleteAsync(StorageDeleteOption.Default);
                _folder = await parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
            }
            catch 
            {
                throw;
            }
        }

        public async Task<string[]> GetKeysAsync() => (await _folder.GetFilesAsync()).Select(x => x.Name).ToArray();

        public async Task<object> LoadAsync(string key) => await FileIO.ReadTextAsync(await GetFile(key));

        public async Task SaveAsync(string key, object value)
        {
            if (value is string s)
            {
                await FileIO.WriteTextAsync(await GetFile(key), s);
            }
            else
            {
                // in order to save to file we require a string
                throw new ArgumentException($"{nameof(value)} must be string : {value}");
            }
        }

        async Task<IStorageFile> GetFile(string key) => await _folder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
    }
}
