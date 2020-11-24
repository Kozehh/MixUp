using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MixUp
{
    class RoomCodeGenerator
    {
        public RoomCodeGenerator()
        {

        }

        public String GenerateRoomCode(IPAddress ip)
        {
            List<int> myList = new List<int>();
            String code = "";
            String ipStr = ip.ToString();
            String[] letters = ipStr.Split();
            return code;
        }


        public IPAddress DecodeRoomCode(String code)
        {
            IPAddress ipAddr = IPAddress.Parse(code);
            return ipAddr;
        }
    }
}
