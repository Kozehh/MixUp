using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    public class Playlist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tracks")]
        public Song[] Songs { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }
    }
}