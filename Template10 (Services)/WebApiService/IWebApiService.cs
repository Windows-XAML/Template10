using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Template10.Services.WebApiService
{
    public interface IWebApiService
    {
        string Serialize<T>(T item);
        T Deserialize<T>(string json);
        Task<string> GetAsync(Uri path);
        Task<HttpResponseMessage> PutAsync<T>(Uri path, T payload);
        Task<HttpResponseMessage> PostAsync<T>(Uri path, T payload);
        Task<HttpResponseMessage> DeleteAsync(Uri path);
    }
}
