using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class LobbyInfo
    {
        [BsonId] [BsonRepresentation(BsonType.ObjectId)] [JsonProperty("id")]
        public string Id { get; set; }

        [BsonElement("RoomCode")]
        [JsonProperty("roomCode")]
        public string RoomCode { get; set; }

        [BsonElement("HostAddress")] [JsonProperty("ipAddr")]
        public string LobbyAddr { get; set; }

    }
}