using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    public class Artist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}