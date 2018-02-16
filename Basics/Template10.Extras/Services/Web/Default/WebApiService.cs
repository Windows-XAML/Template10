using System;
using System.Threading.Tasks;
using Template10.Services.Serialization;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Template10.Services.Web
{
    public class WebApiService : IWebApiService
    {
        private readonly ISerializationService _serializationService;
        private readonly IWebApiAdapter _adapter;

        public WebApiService(ISerializationService serializationService)
        {
            _serializationService = serializationService;
            _adapter = new WindowsHttpClientAdapter(out var client);
            var header = new HttpMediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(header);
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
