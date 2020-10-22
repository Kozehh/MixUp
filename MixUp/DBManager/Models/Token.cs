using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DBManager.Models
{
    public class Token
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("access_token")]
        public string AccessToken { get; set; }

        [BsonElement("token_type")]
        public string TokenType { get; set; }

        [BsonElement("expires_in")]
        public int ExpiresIn { get; set; }

        [BsonElement("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
