using System;
using System.Threading.Tasks;

namespace Template10.Services
{
    public interface IWebApiAdapter
    {
        Task DeleteAsync(Uri path);
        Task<string> GetAsync(Uri path);
        Task PostAsync(Uri path, string payload);
        Task PutAsync(Uri path, string payload);
    }
}