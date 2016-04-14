using Newtonsoft.Json;
using Template10.Samples.IncrementalLoadingSample.Extensions;
using Template10.Samples.IncrementalLoadingSample.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Template10.Samples.IncrementalLoadingSample.Services.GithubService
{
    public class GithubService4RunTime : IGithubService
    {
        public const int PAGE_SIZE = 10;

        public async Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.IsBlank())
            {
                throw new ArgumentException("Github query could not be empty.", nameof(query));
            }

            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex));
            }

            string queryEncoded = WebUtility.UrlEncode(query);
            string url = string.Format("https://api.github.com/search/repositories?q={0}&page={1}&per_page={2}&order=desc", queryEncoded, pageIndex, PAGE_SIZE);

            string json;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAppendWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240");
                json = await client.GetStringAsync(new Uri(url));
            }
            return JsonConvert.DeserializeObject<GithubQueryResult<Repository>>(json);
        }
    }
}