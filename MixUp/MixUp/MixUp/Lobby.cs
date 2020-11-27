using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Xamarin.Forms.Internals;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ClassLibrary.Models;


namespace MixUp
{
    [Serializable]
    public class Lobby : ISerializable
    {
        public String name;
        public List<User> connectedUsers;
        public List<Song> songList;
        public IPAddress ipAddress;
        public Lobby(User host, String lobbyName)
        {
            name = lobbyName;
            connectedUsers = new List<User>();
            songList = new List<Song>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("ipAddress", ipAddress);
            info.AddValue("connectedUsers", connectedUsers);
            info.AddValue("songList", songList);
        }

        public Lobby(SerializationInfo info, StreamingContext context)
        {
            name = (String)info.GetValue("name", typeof(String));
            ipAddress = (IPAddress)info.GetValue("ipAddress", typeof(IPAddress));
            connectedUsers = (List<User>)info.GetValue("connectedUsers", typeof(List<User>));
            songList = (List<Song>)info.GetValue("songList", typeof(List<Song>));
        }
    }
}
