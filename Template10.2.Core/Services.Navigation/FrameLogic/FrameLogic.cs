using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.Navigation
{

    public class FrameLogic : IFrameLogic
    {
        public static IFrameLogic Instance { get; set; } = new FrameLogic();
        private FrameLogic()
        {
            // private constructor
        }

        public async Task<bool> LoadNavigationState(string id, IFrameFacade frame)
        {
            // get state
            var folder = ApplicationData.Current.LocalCacheFolder;
            var file = await folder.TryGetItemAsync(buildKey(id)) as StorageFile;
            if (file == null)
            {
                return false;
            }

            // test date
            var setting = App.Settings.SuspensionStateExpires;
            var expires = DateTime.Now.Subtract(setting);
            var info = await file.GetBasicPropertiesAsync();
            var expired = expires <= info.DateModified;
            if (expired)
            {
                return false;
            }

            // set state
            var state = await FileIO.ReadTextAsync(file);
            frame.SetNavigationState(state);
            return true;
        }

        public async Task<bool> SaveNavigationState(string id, IFrameFacade frame)
        {
            // get state
            var navigationState = frame.GetNavigationState();

            // persist state
            var folder = ApplicationData.Current.LocalCacheFolder;
            var file = await folder.CreateFileAsync(buildKey(id), CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, navigationState);

            return true;
        }

        #region private methods

        private string buildKey(string id) => $"{id}-NavigationState";

        #endregion
    }
}