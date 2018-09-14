using Newtonsoft.Json;

namespace SampleData.Data.StarTrek
{
    public partial class Show 
    {

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonIgnore]
        public Image Image { get; set; }
    }
}