using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Windows.Storage;

namespace Template10.Core
{
    public class FolderPropertyBagPersistenceStrategy : IPropertyBagExPersistenceStrategy
    {
        private StorageFolder folder;

        public FolderPropertyBagPersistenceStrategy(StorageFolder folder) => this.folder = folder;

        public async Task ClearAsync()
        {
            var name = folder.Name;
            var parent = await folder.GetParentAsync();
            await folder.DeleteAsync();
            folder = await parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
        }

        public async Task<string[]> GetKeysAsync() => (await folder.GetFilesAsync()).Select(x => x.Name).ToArray();

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

        async Task<IStorageFile> GetFile(string key) => await folder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
    }
}
