using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Template10.Services
{
    public class WindowsHttpClientAdapter : IWebApiAdapter
    {
        private readonly HttpClient _client;
        private readonly string _mediaType = string.Empty;

        public WindowsHttpClientAdapter(out HttpClient client)
        {
            client = _client = new HttpClient();
        }

        public async Task<string> GetAsync(Uri path)
        {
            var response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task PutAsync(Uri path, string payload)
        {
            var content = new HttpStringContent(payload, UnicodeEncoding.Utf8, _mediaType);
            var result = await _client.PutAsync(path, content);
            result.EnsureSuccessStatusCode();
        }

        public async Task PostAsync(Uri path, string payload)
        {
            var content = new HttpStringContent(payload, UnicodeEncoding.Utf8, _mediaType);
            var result = await _client.PostAsync(path, content);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Uri path)
        {
            var result = await _client.DeleteAsync(path);
            result.EnsureSuccessStatusCode();
        }
    }
}
