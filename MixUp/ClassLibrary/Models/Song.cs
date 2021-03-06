﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class Song
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("album")]
        public Album SongAlbum { get; set; }

        [JsonProperty("duration_ms")]
        public int DurationMs { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}