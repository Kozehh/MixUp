using System;
using System.Collections.Generic;
using ClassLibrary.Util;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class PagingObject<T>
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")] public List<T> Items { get; set; }

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