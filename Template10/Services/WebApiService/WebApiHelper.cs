using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Template10.Services.WebApiService
{
    public class WebApiHelper
    {
        const string applicationJson = "application/json";

        private HttpClient Client()
        {
            var http = new HttpClient();
            var header = new HttpMediaTypeWithQualityHeaderValue(applicationJson);
            http.DefaultRequestHeaders.Accept.Add(header);
            return http;
        }

        public string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<string> GetAsync(Uri path)
        {
            using (var http = Client())
            {
                var response = await http.GetAsync(path);
                if (response.StatusCode == HttpStatusCode.Ok)
                    return await response.Content.ReadAsStringAsync();
                else
                    return string.Empty;
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(Uri path, T payload)
        {
            using (var http = Client())
            {
                var data = Serialize(payload);
                var content = new HttpStringContent(data, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                return await http.PutAsync(path, content);
            }
        }

        public async Task<HttpResponseMessage> PostAsync<T>(Uri path, T payload)
        {
            using (var http = Client())
            {
                var data = Serialize(payload);
                var content = new HttpStringContent(data, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                return await http.PostAsync(path, content);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri path)
        {
            using (var http = Client())
            {
                return await http.DeleteAsync(path);
            }
        }
    }
}
