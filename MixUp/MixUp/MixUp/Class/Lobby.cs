using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace MixUp
{
    [Serializable]
    public class Lobby : ISerializable
    {
        public string name;
        public List<User> connectedUsers;
        public List<Song> songList;
        public IPAddress ipAddress;
        public string roomCode;
        public bool notPlayed;
        public Song currentPlayingSong;
        public Song nextInQueue;

        public Lobby(User host, string lobbyName, string code)
        {
            roomCode = code;
            name = lobbyName;
            connectedUsers = new List<User>();
            songList = new List<Song>();
            currentPlayingSong = null;
            nextInQueue = null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("roomCode", roomCode);
            info.AddValue("name", name);
            info.AddValue("ipAddress", ipAddress);
            info.AddValue("connectedUsers", connectedUsers);
            info.AddValue("songList", songList);
            info.AddValue("currentPlayingSong", currentPlayingSong);
            info.AddValue("nextInQueue", nextInQueue);
        }

        public Lobby(SerializationInfo info, StreamingContext context)
        {
            roomCode = (string)info.GetValue("roomCode", typeof(string));
            name = (string)info.GetValue("name", typeof(string));
            ipAddress = (IPAddress)info.GetValue("ipAddress", typeof(IPAddress));
            connectedUsers = (List<User>)info.GetValue("connectedUsers", typeof(List<User>));
            songList = (List<Song>)info.GetValue("songList", typeof(List<Song>));
            currentPlayingSong = (Song) info.GetValue("currentPlayingSong", typeof(Song));
            nextInQueue = (Song) info.GetValue("nextInQueue", typeof(Song));
        }
    }
}
