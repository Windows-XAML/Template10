using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;
using Template10.Navigation;
using Windows.Storage;

namespace Template10.Strategies
{
    public class DefaultNavStateStrategy : INavStateStrategy
    {
        private async Task<StorageFolder> GetRootAsync()
        {
            var folder = ApplicationData.Current.TemporaryFolder;
            return await folder.CreateFolderAsync("~Template10", CreationCollisionOption.OpenIfExists);
        }

        private async Task<StorageFolder> GetFrameRootAsync(string frameId)
        {
            var folder = await GetRootAsync();
            return await folder.CreateFolderAsync(frameId, CreationCollisionOption.OpenIfExists);
        }

        private async Task<StorageFolder> GetPageRootAsync(string frameId, string pageId)
        {
            var folder = await GetFrameRootAsync(frameId);
            return await folder.CreateFolderAsync(pageId, CreationCollisionOption.OpenIfExists);
        }

        public async Task<FrameExState> GetFrameStateAsync(string frameId)
        {
            var folder = await GetFrameRootAsync(frameId);
            var store = PropertyBagEx.Create(folder);
            return new FrameExState(store);
        }

        public async Task<IPropertyBagEx> GetPageStateAsync(string frameId, string pageId)
        {
            var folder = await GetPageRootAsync(frameId, pageId);
            return PropertyBagEx.Create(folder);
        }

        public async Task ClearAsync()
        {
            await (await GetRootAsync()).DeleteAsync();
        }
    }

}
