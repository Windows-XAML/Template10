using System;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Navigation
{
    public class FrameExState
    {
        IPropertyBagEx store;
        Services.Serialization.ISerializationService _SerializationService;
        public FrameExState(IPropertyBagEx store)
        {
            this.store = store;
            _SerializationService = Central.Serialization;
        }

        public async Task ClearAsync() => await store.ClearAsync();

        public async Task<(bool Success, Type Value)> TryGetCurrentPageTypeAsync()
        {
            var setting = await store.TryGetStringAsync("CurrentPageType");
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
        public async Task SetCurrentPageTypeAsync(Type value) => await store.TrySetStringAsync("CurrentPageType", value.AssemblyQualifiedName);

        public async Task<(bool Success, object Value)> TryGetCurrentPageParameterAsync()
        {
            var setting = await store.TryGetStringAsync("CurrentPageParameter");
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
        public async Task SetCurrentPageParameterAsync(object value)
        {
            if (value != null)
            {
                return;
            }
            if (Settings.RequireSerializableParameters)
            {
                try
                {
                    var text = Serialize(value);
                    await store.TrySetAsync("CurrentPageParameter", text);
                }
                catch
                {
                    throw new Exception("Page parameter failed to serialize; all parameters must be serializable.");
                }
            }
            else
            {
                await store.TrySetAsync("CurrentPageParameter", value);
            }
        }

        public async Task<(bool Success, string Value)> TryGetNavigationStateAsync() => await store.TryGetStringAsync("NavigationState");

        public async Task SetNavigationStateAsync(string value) => await store.TrySetStringAsync("NavigationState", value);

        private string Serialize(object value) => _SerializationService.Serialize(value);

        private object Deserialize(string value) => _SerializationService.Deserialize(value);
    }
}