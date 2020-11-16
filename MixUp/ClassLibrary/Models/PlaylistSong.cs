using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    public class PlaylistSong<T>
    {
        [JsonProperty("track")]
        public T PlaylistSongs { get ; set; }
    }
}