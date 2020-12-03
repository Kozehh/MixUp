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
        public bool CreateLobby([FromBody] LobbyInfo lobbyInfo)
        {
            bool canSaveLobby = false;
            var result = _lobbyService.Exists(lobbyInfo);
            // Si le code généré n'est pas déjà associé à un lobby
            if (result == null)
            {
                canSaveLobby = true;
                // On peut en créer un nouveau
                _lobbyService.CreateLobby(lobbyInfo);
            }
            // Sinon on retourne l'objet avec une adresse null, et on va en générer un autre
            return canSaveLobby;
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
