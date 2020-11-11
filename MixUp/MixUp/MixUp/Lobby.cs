using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using Xamarin.Forms.Internals;


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ClassLibrary.Models;


namespace MixUp
{
    public class Lobby 
    {
        public List<User> connectedUsers;
        public List<Song> songList;
        public IPAddress ipAddress;
        public Lobby(String hostName)
        {
            connectedUsers = new List<User>();
            songList = new List<Song>();
        }

        public Lobby()
        {

        public Lobby(SerializationInfo info, StreamingContext context)
        {
            ipAddress = (IPAddress)info.GetValue("ipAddress", typeof(IPAddress));
            connectedUsers = (List<User>)info.GetValue("connectedUsers", typeof(List<User>));
            songList = (List<Song>)info.GetValue("songList", typeof(List<Song>));
        }


    }
}
