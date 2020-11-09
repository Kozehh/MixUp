using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using Xamarin.Forms.Internals;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace MixUp
{
    [Serializable()]
    public class Lobby : ISerializable
    {
        public List<String> connectedUsers;
        public List<String> songList;
        public IPAddress ipAddress;
        public Lobby(String hostName)
        {
            connectedUsers = new List<String>();
            songList = new List<String>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ipAddress", ipAddress);
            info.AddValue("connectedUsers", connectedUsers);
            info.AddValue("connectedUsers", songList);
        }

        public Lobby(SerializationInfo info, StreamingContext context)
        {
            ipAddress = (IPAddress)info.GetValue("ipAddress", typeof(IPAddress));
            connectedUsers = (List<String>)info.GetValue("connectedUsers", typeof(List<String>));
            songList = (List<String>)info.GetValue("songList", typeof(List<String>));
        }
    }
}
