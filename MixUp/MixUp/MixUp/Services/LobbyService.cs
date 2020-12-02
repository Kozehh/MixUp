using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ClassLibrary.Models;
using Newtonsoft.Json;

namespace MixUp.Services
{
    public class LobbyService
    {
        private HttpClient client = new HttpClient();
        public async void SaveLobbyCode(string roomCode, string ipAddr)
        {
            try
            {
                LobbyInfo lobbyInfo = new LobbyInfo()
                {
                    LobbyAddr = ipAddr,
                    RoomCode = roomCode
                };
                var serialize = JsonConvert.SerializeObject(lobbyInfo);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var res = client.PostAsync("ADRESSEMIXUP", toSend);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
