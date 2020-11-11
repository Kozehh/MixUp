using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [JsonObject]
    public class PagingObject
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public string NextPageUrl { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public string PreviousPageUrl { get; set; }

        [JsonProperty("total")]
        public int TotalAvailable { get; set; }
    }
}