using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    public class Song
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("duration_ms")]
        public int DurationMs { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}