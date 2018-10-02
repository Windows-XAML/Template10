using Newtonsoft.Json;

namespace Template10.SampleData.StarTrek
{
    public partial class Ship 
    {
        [JsonProperty("show")]
        public string Show { get; set; }

        [JsonProperty("registry")]
        public string Registry { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Image Image { get; set; }
    }
}