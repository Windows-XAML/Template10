namespace Template10.SampleData.Food
{
    using Newtonsoft.Json;

    public partial class JsonRoot
    {
        [JsonProperty("fruit")]
        public Fruit[] Fruit { get; set; }
    }

}
