using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Common.PersistedDictionary
{
    public class PersistedDictionaryFolderAdapter : PersistedDictionaryBase
    {
        private StorageFolder folder;

        public async override Task ClearAsync()
        {
            var name = folder.Name;
            var parent = await folder.GetParentAsync();
            await folder.DeleteAsync();
            folder = await parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
        }

        public PersistedDictionaryFolderAdapter(StorageFolder folder) : base(folder.Name)
            => this.folder = folder;

        public override async Task<string[]> AllKeysAsync()
        {
            var files = await folder.GetFilesAsync();
            return files.Select(x => x.Name).ToArray();
        }

        public override async Task<string> GetStringAsync(string key)
        {
            if (!await ContainsKeyAsync(key))
            {
                throw new KeyNotFoundException(key);
            }
            var file = await folder.GetFileAsync(key);
            return await FileIO.ReadTextAsync(file);
        }

        public override async Task SetStringAsync(string key, string value)
        {
            var file = await folder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, value);
            base.RaiseMapChanged(key);
        }
    }
}
