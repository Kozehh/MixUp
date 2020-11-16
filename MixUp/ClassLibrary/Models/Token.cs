using MongoDB.Bson.Serialization.Attributes;
using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class Token
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [BsonElement("AccessToken")]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [BsonElement("TokenType")]
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [BsonElement("ExpiresIn")]
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [BsonElement("RefreshToken")]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
