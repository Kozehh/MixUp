using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace MixUp
{
    [Serializable()]
    public class Lobby : ISerializable
    {
        public String name;
        public List<User> connectedUsers;
        public List<Song> songList;
        public IPAddress ipAddress;
        public String roomCode;
        public Lobby(User host, String lobbyName, String code)
        {
            roomCode = code;
            name = lobbyName;
            connectedUsers = new List<User>();
            songList = new List<Song>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("roomCode", roomCode);
            info.AddValue("name", name);
            info.AddValue("ipAddress", ipAddress);
            info.AddValue("connectedUsers", connectedUsers);
            info.AddValue("songList", songList);
        }

        public Lobby(SerializationInfo info, StreamingContext context)
        {
            roomCode = (String)info.GetValue("roomCode", typeof(String));
            name = (String)info.GetValue("name", typeof(String));
            ipAddress = (IPAddress)info.GetValue("ipAddress", typeof(IPAddress));
            connectedUsers = (List<User>)info.GetValue("connectedUsers", typeof(List<User>));
            songList = (List<Song>)info.GetValue("songList", typeof(List<Song>));
        }
    }
}
