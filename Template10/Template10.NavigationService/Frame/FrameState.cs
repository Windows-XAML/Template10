using System;
using System.Threading.Tasks;
using Template10.Portable.PersistedDictionary;

namespace Template10.Services.NavigationService
{
    public class FrameState
    {
        IPersistedDictionary store;
        public FrameState(IPersistedDictionary store) => this.store = store;

        public async Task ClearAsync()
        {
            await store.ClearAsync();
        }

        public async Task<(bool Success, DateTime Value)> TryGetCacheDateKeyAsync()
        {
            var setting = await store.TryGetValueAsync("CacheDateKey");
            if (!setting.Success)
            {
                return (false, DateTime.MinValue);
            }
            try
            {
                return (true, DateTime.Parse(setting.Value));
            }
            catch (Exception)
            {
                return (false, DateTime.MinValue);
            }
        }
        public async Task SetCacheDateKeyAsync(DateTime value) => await store.SetValueAsync("CacheDateKey", value.ToString());

        public async Task<(bool Success, Type Value)> TryGetCurrentPageTypeAsync()
        {
            var setting = await store.TryGetValueAsync("CurrentPageType");
            if (!setting.Success)
            {
                return (false, null);
            }
            try
            {
                return (true, Type.GetType(setting.Value));
            }
            catch (Exception)
            {
                return (false, null);
            }
        }
        public async Task SetCurrentPageTypeAsync(Type value) => await store.SetValueAsync("CurrentPageType", value.AssemblyQualifiedName);

        public async Task<(bool Success, object Value)> TryGetCurrentPageParameterAsync()
        {
            var setting = await store.TryGetValueAsync("CurrentPageParameter");
            if (!setting.Success)
            {
                return (false, null);
            }
            try
            {
                return (true, Deserialize(setting.Value));
            }
            catch (Exception)
            {
                return (false, null);
            }
        }
        public async Task SetCurrentPageParameterAsync(object value) => await store.SetValueAsync("CurrentPageParameter", Serialize(value));

        public async Task<(bool Success, string Value)> TryGetNavigationStateAsync() => await store.TryGetValueAsync("NavigationState");
        public async Task SetNavigationStateAsync(string value) => await store.SetValueAsync("NavigationState", value);

        private string Serialize(object value) => Settings.SerializationStrategy.Serialize(value);
        private object Deserialize(string value) => Settings.SerializationStrategy.Deserialize(value);
    }
}