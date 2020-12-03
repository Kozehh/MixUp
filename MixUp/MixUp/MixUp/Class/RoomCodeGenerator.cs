using System;
using System.Linq;
using System.Net;
using ClassLibrary.Models;
using MixUp.Services;

namespace MixUp
{
    class RoomCodeGenerator
    {
        private LobbyService lobbyService;
        public RoomCodeGenerator()
        {
            lobbyService = new LobbyService();
        }
        
        public string GenerateAndInsertCode(IPAddress ip)
        {
            string ipString = ip.ToString();
            string generatedCode = "";
            do
            {
                generatedCode = RandomString(6);
            } while (!lobbyService.SaveLobbyCode(generatedCode, ipString));
            
            return generatedCode;
        }

        public LobbyInfo GetRoomAddress(string code)
        {
            code = code.ToUpper();
            return lobbyService.FindLobbyWithCode(code);
        }


        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
