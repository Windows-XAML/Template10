using System;
using System.Threading.Tasks;

namespace Template10.Services
{
    public interface IWebApiService
    {
        Task<string> GetAsync(Uri path);

        Task PutAsync<T>(Uri path, T payload);

        Task PostAsync<T>(Uri path, T payload);

        Task DeleteAsync(Uri path);
    }
}
