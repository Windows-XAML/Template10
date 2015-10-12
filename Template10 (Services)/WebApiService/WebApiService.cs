using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Template10.Services.WebApiService
{
    public class WebApiService : IWebApiService
    {
        WebApiHelper _helper = new WebApiHelper();

        public string Serialize<T>(T item) => _helper.Serialize<T>(item);

        public T Deserialize<T>(string json) => _helper.Deserialize<T>(json);

        public Task<string> GetAsync(Uri path) => _helper.GetAsync(path);

        public Task<HttpResponseMessage> PutAsync<T>(Uri path, T payload) => _helper.PutAsync<T>(path, payload);

        public Task<HttpResponseMessage> PostAsync<T>(Uri path, T payload) => _helper.PostAsync(path, payload);

        public Task<HttpResponseMessage> DeleteAsync(Uri path) => _helper.DeleteAsync(path);
    }
}
