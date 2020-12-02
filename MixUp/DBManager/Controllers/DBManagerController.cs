using System;
using ClassLibrary.Models;
using DBManager.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DBManager.Controllers
{
    [Route("db-manager")]
    [ApiController]
    public class DBManagerController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly LobbyService _lobbyService;

        public DBManagerController(UserService userService, LobbyService lobbyService)
        {
            _userService = userService;
            _lobbyService = lobbyService;
        }

        [HttpGet]
        [Route("db")]
        public void Test()
        {
            var client = new MongoClient("mongodb+srv://dbAnthoAdmin:xgf1jm3gYRTpqlcP@mixup.wrwba.mongodb.net/MixUp?retryWrites=true&w=majority");
            var dbL = client.GetDatabase("sample_airbnb");
            var ll = dbL.ListCollections().ToList();
            foreach (var doc in ll)
            {
                Console.WriteLine(doc.ToString());
            }
        }

        [HttpPost]
        [Route("Token/Update")]
        public void UpdateToken([FromBody] Token newToken)
        {
            UserService service = new UserService();
        }

        [HttpPost]
        [Route("Token/Add")]
        public Token AddToken([FromBody] Token newToken)
        {
            return _userService.Create(newToken);
        }

        [HttpPost]
        [Route("Lobby/Code")]
        public void CreateLobby([FromBody] LobbyInfo lobbyInfo)
        {
            _lobbyService.CreateLobby(lobbyInfo);
        }
    }
}
