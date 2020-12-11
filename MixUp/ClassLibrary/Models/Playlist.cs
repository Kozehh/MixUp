using System;
using System.Collections.Generic;
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

        public PagingObject<PlaylistSong<Song>> PlaylistSongs { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}