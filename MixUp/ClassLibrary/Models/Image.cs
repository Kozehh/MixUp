using System;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}