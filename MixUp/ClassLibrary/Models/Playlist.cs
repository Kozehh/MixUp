using System.Collections.Generic;
using ClassLibrary.Util;
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
        [JsonConverter(typeof(JsonListConverter))]
        public List<Song> Songs { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }
    }
}