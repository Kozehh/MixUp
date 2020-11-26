using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class Album
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tracks")]
        public List<Song> Songs { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }
    }
}