using System;
using System.Linq;
using System.Net;

namespace MixUp
{
    class RoomCodeGenerator
    {
        public RoomCodeGenerator(){}

        public string GenerateAndInsertCode(IPAddress ip)
        {
            string ipString = ip.ToString();
            string generatedCode = "";
            do
            {
                generatedCode = RandomString(6);
            }
            //while(BD.exist(generatedCode));
            while (false);

            // BD.insert(roomCode, ipString);
            
            return generatedCode;
        }

        public string GetRoomAddress(string code)
        {
            code = code.ToUpper();
            string ipAddStr = "";
            // if(BD.exist(code))
            //{
            // ipAddStr = BD.getRoomCode(code)
            //}

            return ipAddStr;
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
