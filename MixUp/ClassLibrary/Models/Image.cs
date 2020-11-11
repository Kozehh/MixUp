using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}