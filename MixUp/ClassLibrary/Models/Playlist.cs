using System;
using System.Collections.Generic;
using ClassLibrary.Util;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class Playlist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }
    }
}