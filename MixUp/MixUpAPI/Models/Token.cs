using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MixUpAPI.Models
{
    public class Token
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }
    }
}
