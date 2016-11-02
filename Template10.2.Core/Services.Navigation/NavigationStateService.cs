using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.Navigation
{

    public class NavigationStateService : INavigationStateService
    {
        public static NavigationStateService Instance { get; } = new NavigationStateService();
        private NavigationStateService()
        {
            // private constructor
        }

        public async Task<string> LoadFromCacheAsync(string frameId)
        {
            var key = $"{frameId}-NavigationState";
            var folder = ApplicationData.Current.LocalCacheFolder;
            var file = await folder.TryGetItemAsync(key) as StorageFile;
            if (file == null)
            {
                return string.Empty;
            }
            else
            {
                var expiry = App.Settings.RestoreExpires;
                var expires = DateTime.Now.Subtract(expiry);
                var info = await file.GetBasicPropertiesAsync();
                if (expires > info.DateModified)
                {
                    var navigationState = await FileIO.ReadTextAsync(file);
                    var state = await FileIO.ReadTextAsync(file);
                    return state;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public async Task<bool> SaveToCacheAsync(string frameId, string state)
        {
            var key = $"{frameId}-NavigationState";
            var folder = ApplicationData.Current.LocalCacheFolder;
            var file = await folder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, state);
            return true;
        }
    }
}