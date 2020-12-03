
using System;
using ClassLibrary.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DBManager.Services
{
    public class LobbyService
    {
        private readonly IMongoCollection<LobbyInfo> _connectionInfo;
        public LobbyService()
        {
            var client = new MongoClient("mongodb+srv://dbAnthoAdmin:xgf1jm3gYRTpqlcP@mixup.wrwba.mongodb.net/MixUp?retryWrites=true&w=majority");
            var database = client.GetDatabase("Lobby");

            _connectionInfo = database.GetCollection<LobbyInfo>("ConnectionInfo");
        }

        public LobbyInfo Get(LobbyInfo lobby) =>
            _connectionInfo.Find<LobbyInfo>(room => room.RoomCode == lobby.RoomCode).FirstOrDefault();

        public LobbyInfo Exists(LobbyInfo lobby) =>
            _connectionInfo.Find<LobbyInfo>(room => room.RoomCode == lobby.RoomCode).Limit(1).FirstOrDefault();

        public void CreateLobby(LobbyInfo lobbyInfo)
        {
            lobbyInfo.Id = ObjectId.GenerateNewId().ToString();
            _connectionInfo.InsertOne(lobbyInfo);
        }
    }
}
