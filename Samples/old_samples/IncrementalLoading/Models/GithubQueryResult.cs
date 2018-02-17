using Newtonsoft.Json;

namespace Template10.Samples.IncrementalLoadingSample.Models
{
    [JsonObject]
    public class GithubQueryResult<T>
    {
        [JsonProperty("total_count")]
        public int TotalCount
        {
            get;
            set;
        }

        [JsonProperty("incomplete_results")]
        public bool IncompleteResults
        {
            get;
            set;
        }

        [JsonProperty("items")]
        public T[] Items
        {
            get;
            set;
        }

        [JsonProperty("message")]
        public string ErrorMessage
        {
            get;
            set;
        }
    }
}