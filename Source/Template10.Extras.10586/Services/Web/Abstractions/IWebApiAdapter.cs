using System;
using System.Threading.Tasks;

namespace Template10.Services.Web
{
    public interface IWebApiAdapter
    {
        void AddHeader(string key, string value);
        Task DeleteAsync(Uri path);
        Task<string> GetAsync(Uri path);
        Task PostAsync(Uri path, string payload);
        Task PutAsync(Uri path, string payload);
    }
}