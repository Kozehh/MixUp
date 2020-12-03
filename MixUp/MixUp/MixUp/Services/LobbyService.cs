using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using MixUp.Pages;
using Newtonsoft.Json;

namespace MixUp.Services
{
    public class LobbyService
    {
        private HttpClient client = new HttpClient();
        public bool SaveLobbyCode(string roomCode, string ipAddr)
        {
            bool canSaveLobby = true;
            try
            {
                LobbyInfo lobbyInfo = new LobbyInfo()
                {
                    LobbyAddr = ipAddr,
                    RoomCode = roomCode
                };
                var serialize = JsonConvert.SerializeObject(lobbyInfo);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var res = client.PostAsync(LoginPage.mixupApi+"lobby/create", toSend).Result;
                canSaveLobby = JsonConvert.DeserializeObject<bool>(res.Content.ReadAsStringAsync().Result);
                return canSaveLobby;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return canSaveLobby = false;
            }
        }

        public LobbyInfo FindLobbyWithCode(string roomCode)
        {
            LobbyInfo lobby = new LobbyInfo()
            {
                RoomCode = roomCode
            };
            try
            {
                var serialize = JsonConvert.SerializeObject(lobby);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var result = client.PostAsync(LoginPage.mixupApi + "lobby/connect", toSend).Result;
                lobby = JsonConvert.DeserializeObject<LobbyInfo>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lobby;
        }
    }
}
