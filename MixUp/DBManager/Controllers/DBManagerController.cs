using System;
using ClassLibrary.Models;
using DBManager.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DBManager.Controllers
{
    [Route("dbmanager")]
    [ApiController]
    public class DBManagerController : ControllerBase
    {

        private UserService _userService;
        private LobbyService _lobbyService;

        public DBManagerController()
        {
            _userService = new UserService();
            _lobbyService = new LobbyService();
        }

        
        [HttpPost]
        [Route("token/update")]
        public void UpdateToken([FromBody] Token newToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("token/add")]
        public Token AddToken([FromBody] Token newToken)
        {
            return _userService.Create(newToken);
        }

        [HttpPost]
        [Route("lobby/create")]
        public void CreateLobby([FromBody] LobbyInfo lobbyInfo)
        {
            _lobbyService.CreateLobby(lobbyInfo);
        }

        [HttpPost]
        [Route("lobby/connect")]
        public LobbyInfo GetLobbyAddr([FromBody] LobbyInfo lobby)
        {
            var lobbyInfo = _lobbyService.Get(lobby);
            return lobbyInfo;
        }
    }
}
