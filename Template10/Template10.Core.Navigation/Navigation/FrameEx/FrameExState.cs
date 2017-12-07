using System;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Navigation
{
    public class FrameExState
    {
        IPropertyBagEx _store;
        Services.Serialization.ISerializationService _serializationService;

        public FrameExState(IPropertyBagEx store)
        {
            this._store = store;
            _serializationService = Central.Serialization;
        }

        public async Task ClearAsync() => await _store.ClearAsync();

        public async Task<TryResult<Type>> TryGetCurrentPageTypeAsync()
        {
            var setting = await _store.TryGetStringAsync("CurrentPageType");
            if (!setting.Success)
            {
                return new TryResult<Type>();
            }
            try
            {
                return new TryResult<Type> { Success = true, Value = Type.GetType(setting.Value) };
            }
            catch (Exception)
            {
                return new TryResult<Type>();
            }
        }
        public async Task SetCurrentPageTypeAsync(Type value) => await _store.TrySetStringAsync("CurrentPageType", value.AssemblyQualifiedName);

        public async Task<TryResult<object>> TryGetCurrentPageParameterAsync()
        {
            var setting = await _store.TryGetStringAsync("CurrentPageParameter");
            if (!setting.Success)
            {
                return new TryResult<object>();
            }
            try
            {
                return new TryResult<object> { Success = true, Value = Deserialize(setting.Value) };
            }
            catch (Exception)
            {
                return new TryResult<object>();
            }
        }
        public async Task SetCurrentPageParameterAsync(object value)
        {
            if (value != null)
            {
                return;
            }
            await _store.TrySetAsync("CurrentPageParameter", value);
        }

        public async Task<TryResult<string>> TryGetNavigationStateAsync() 
            => await _store.TryGetStringAsync("NavigationState");

        public async Task SetNavigationStateAsync(string value) 
            => await _store.TrySetStringAsync("NavigationState", value);

        private string Serialize(object value) 
            => _serializationService.Serialize(value);

        private object Deserialize(string value) 
            => _serializationService.Deserialize(value);
    }
}