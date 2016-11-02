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
                var expiry = App.Settings.SuspensionStateExpires;
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

        public async Task<bool> LoadNavigationState(String id, IFrameFacade frame)
        {
            // get state
            var key = $"{id}-NavigationState";
            var folder = ApplicationData.Current.LocalCacheFolder;
            var file = await folder.TryGetItemAsync(key) as StorageFile;
            string navigationState = String.Empty;
            if (file == null)
            {
                return false;
            }
            else
            {
                var expiry = App.Settings.SuspensionStateExpires;
                var expires = DateTime.Now.Subtract(expiry);
                var info = await file.GetBasicPropertiesAsync();
                if (expires > info.DateModified)
                {
                    navigationState = await FileIO.ReadTextAsync(file);
                }
                else
                {
                    return false;
                }
            }

            // set state
            frame.SetNavigationState(navigationState);

            return true;
        }

        public async Task<bool> SaveNavigationState(String id, IFrameFacade frame)
        {
            // get state
            var navigationState = frame.GetNavigationState();

            // persist state
            var key = $"{id}-NavigationState";
            var folder = ApplicationData.Current.LocalCacheFolder;
            var file = await folder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, navigationState);

            return true;
        }
    }
}