using System;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class Album
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tracks")]
        public Song[] Songs { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

    }
}