using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MixUp
{
    class RoomCodeGenerator
    {
        public RoomCodeGenerator()
        {

        }

        public String GenerateAndInsertCode(IPAddress ip)
        {
            String ipString = ip.ToString();
            String generatedCode = "";
            do
            {
                generatedCode = RandomString(6);
            }
            //while(BD.exist(generatedCode));
            while (false);

            // BD.insert(roomCode, ipString);
            
            return generatedCode;
        }

        public String GetRoomAddress(String code)
        {
            code = code.ToUpper();
            String ipAddStr = "";
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
