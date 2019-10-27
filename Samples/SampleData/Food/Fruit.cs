namespace Template10.SampleData.Food
{
    using Newtonsoft.Json;

    public partial class Fruit
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        public Image Image { get; set; }
    }

}
