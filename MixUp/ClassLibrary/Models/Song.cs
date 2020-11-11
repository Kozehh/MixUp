using System.Collections.Generic;
using ClassLibrary.Util;
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
        [JsonConverter(typeof(JsonListConverter))]
        public List<Artist> Artists { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}