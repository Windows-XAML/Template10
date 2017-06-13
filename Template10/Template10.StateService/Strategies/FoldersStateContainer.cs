using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.StateService
{
    public enum FoldersStateLocations { Local, Roam }

    public class FoldersStateContainer : StateContainerBase
    {
        public static async Task<FoldersStateContainer> CreateAsync(FoldersStateLocations location, string container = null, string key = null)
        {
            key = key ?? nameof(FoldersStateContainer);
            var root = default(StorageFolder);
            switch (location)
            {
                case FoldersStateLocations.Local:
                    root = ApplicationData.Current.LocalFolder;
                    break;
                case FoldersStateLocations.Roam:
                    root = ApplicationData.Current.RoamingFolder;
                    break;
            }
            root = await root.CreateFolderAsync("App-State", CreationCollisionOption.OpenIfExists);
            if (!string.IsNullOrEmpty(container))
            {
                root = await root.CreateFolderAsync(container, CreationCollisionOption.OpenIfExists);
            }
            var folder = await root.CreateFolderAsync(key, CreationCollisionOption.OpenIfExists);
            return new FoldersStateContainer(folder);
        }

        private StorageFolder folder;

        public async override Task ClearAsync()
        {
            var name = folder.Name;
            var parent = await folder.GetParentAsync();
            await folder.DeleteAsync();
            folder = await parent.CreateFolderAsync(name, CreationCollisionOption.OpenIfExists);
        }

        private FoldersStateContainer(StorageFolder folder) : base(folder.Name)
            => this.folder = folder;

        public override async Task<string[]> AllKeysAsync()
        {
            var files = await folder.GetFilesAsync();
            return files.Select(x => x.Name).ToArray();
        }

        public override async Task<string> GetValueAsync(string key)
        {
            if (!await ContainsKeyAsync(key))
            {
                throw new KeyNotFoundException(key);
            }
            var file = await folder.GetFileAsync(key);
            return await FileIO.ReadTextAsync(file);
        }

        public override async Task SetValueAsync(string key, string value)
        {
            var file = await folder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, value);
        }
    }
}
