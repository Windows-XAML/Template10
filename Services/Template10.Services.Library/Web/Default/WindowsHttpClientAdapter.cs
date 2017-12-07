using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Template10.Services.Web
{
    public class WindowsHttpClientAdapter : IWebApiAdapter
    {
        static HttpClient _client;
        private string _mediaType;

        static WindowsHttpClientAdapter()
        {
            _client = new HttpClient();
        }

        public WindowsHttpClientAdapter(string mediaType = "application/json")
        {
            _mediaType = mediaType;
            var header = new HttpMediaTypeWithQualityHeaderValue(mediaType);
            _client.DefaultRequestHeaders.Accept.Add(header);
        }

        public async Task<string> GetAsync(Uri path)
        {
            var response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task PutAsync(Uri path, string payload)
        {
            var content = new HttpStringContent(payload, Windows.Storage.Streams.UnicodeEncoding.Utf8, _mediaType);
            var result = await _client.PutAsync(path, content);
            result.EnsureSuccessStatusCode();
        }

        public async Task PostAsync(Uri path, string payload)
        {
            var content = new HttpStringContent(payload, Windows.Storage.Streams.UnicodeEncoding.Utf8, _mediaType);
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
