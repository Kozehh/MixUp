using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [BsonElement("Email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [BsonElement("Country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("href")]
        public string UserApi { get; set; }

        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("product")]
        public string Subscription { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("uri")]
        public string UserUri { get; set; }
        
        [JsonProperty("images")]
        public List<Image> ProfileImages { get; set; }
        /*
        [JsonProperty("explicit_content")]
        public bool Explicit { get; set; }
        */
        [JsonProperty("external_urls")]
        public ExternalUrl Urls { get; set; }

        [JsonProperty("followers")]
        public Follower Followers { get; set; }
    }

    [JsonArray]
    public class Image
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Follower
    {
        [JsonProperty("href")]
        public string UserApi { get; set; }

        [JsonProperty("total")]
        public int Followers { get; set; }
    }

    public class ExternalUrl
    {
        [JsonProperty("spotify")]
        public Dictionary<string, string> Url { get; set; }
    }
}
