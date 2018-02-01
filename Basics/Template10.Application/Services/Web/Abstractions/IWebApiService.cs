using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Prism.Windows.Services.Web
{
    public interface IWebApiService
    {
        Task<string> GetAsync(Uri path);

        Task PutAsync<T>(Uri path, T payload);

        Task PostAsync<T>(Uri path, T payload);

        Task DeleteAsync(Uri path);
    }
}
