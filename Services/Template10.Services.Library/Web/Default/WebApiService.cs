using System;
using System.Threading.Tasks;
using Template10.Services.Serialization;
using Windows.Web.Http;

namespace Template10.Services.Web
{
    public class WebApiService : IWebApiService
    {
        private ISerializationService _serializationService;
        private IWebApiAdapter _adapter;

        public WebApiService(ISerializationService serializationService, IWebApiAdapter helper = null)
        {
            _serializationService = serializationService;
            _adapter = helper ?? new WindowsHttpClientAdapter();
        }

        public async Task<string> GetAsync(Uri path)
        {
            return await _adapter.GetAsync(path);
        }

        public async Task PutAsync<T>(Uri path, T payload)
        {
            var value = _serializationService.Serialize(payload);
            await _adapter.PutAsync(path, value);
        }

        public async Task PostAsync<T>(Uri path, T payload)
        {
            var value = _serializationService.Serialize(payload);
            await _adapter.PostAsync(path, value);
        }

        public async Task DeleteAsync(Uri path)
        {
            await _adapter.DeleteAsync(path);
        }
    }
}
