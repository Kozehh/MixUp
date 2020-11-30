using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<PlayerDevice> Devices { get; set; }

        public Token Token;

        [BsonElement("Name")]
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [BsonElement("Email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("href")]
        public string UserApi { get; set; }

        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("product")]
        public string Subscription { get; set; }

        [JsonProperty("uri")]
        public string UserUri { get; set; }

        public List<Playlist> UserPlaylists { get; set; }
    }
}
