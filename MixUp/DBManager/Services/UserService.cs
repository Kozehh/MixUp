using System.Collections.Generic;
using DBManager.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DBManager.Services
{
    public class UserService
    {
        private readonly IMongoCollection<Token> _token;

        public UserService()
        {
            var client = new MongoClient("mongodb+srv://dbAnthoAdmin:xgf1jm3gYRTpqlcP@mixup.wrwba.mongodb.net/MixUp?retryWrites=true&w=majority");
            var database = client.GetDatabase("User");

            _token = database.GetCollection<Token>("Token");
        }

        public List<Token> Get() =>
            _token.Find(token => true).ToList();

        public Token Get(string id) =>
            _token.Find<Token>(token => token.Id == id).FirstOrDefault();

        public Token Create(Token token)
        {
            token.Id = ObjectId.GenerateNewId().ToString();
            _token.InsertOne(token);
            return token;
        }

        public void Update(string id, Token tokenIn) =>
            _token.ReplaceOne(token => token.Id == id, tokenIn);
    }
}
