using Newtonsoft.Json;
using System;

namespace Template10.Samples.IncrementalLoadingSample.Models
{
    [JsonObject]
    public class Repository
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Full name of this repository.
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Home page url.
        /// </summary>
        [JsonProperty("html_url")]
        public string HomeUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Last updated time.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Main code language of this repository.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// How many stars this repository got.
        /// </summary>
        [JsonProperty("stargazers_count")]
        public int Star { get; set; }
    }
}