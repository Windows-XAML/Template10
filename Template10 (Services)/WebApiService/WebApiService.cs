using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Template10.Services.WebApiService
{
    public class WebApiService : IWebApiService
    {
        WebApiHelper _helper = new WebApiHelper();

        public string Serialize<T>(T item)
        {
            return _helper.Serialize<T>(item);
        }

        public T Deserialize<T>(string json)
        {
            return _helper.Deserialize<T>(json);
        }

        public Task<string> GetAsync(Uri path)
        {
            return _helper.GetAsync(path);
        }

        public Task<HttpResponseMessage> PutAsync<T>(Uri path, T payload)
        {
            return _helper.PutAsync<T>(path, payload);
        }

        public Task<HttpResponseMessage> PostAsync<T>(Uri path, T payload)
        {
            return _helper.PostAsync(path, payload);
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri path)
        {
            return _helper.DeleteAsync(path);
        }
    }
}
