using System;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class PlaylistSong<T>
    {
        [JsonProperty("track")]
        public T Song { get ; set; }

        public override string ToString()
        {
            return this.Song.ToString();
        }
    }
}