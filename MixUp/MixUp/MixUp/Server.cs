using System;
using System.Collections.Generic;
using System.Text;

// A C# Program for Server 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ClassLibrary.Models;

namespace MixUp
{
    public class Server
    {
        public Lobby serverLobby;

        public List<Thread> serverThreads;

        public List<Socket> connectedUsersList;

        public ServerConnectionManager serverConnectionManager;
        public User _userHost;

        public Server(User user)
        {
            _userHost = user;
            serverLobby = new Lobby(user);
            serverThreads = new List<Thread>();
            connectedUsersList = new List<Socket>();
            serverConnectionManager = new ServerConnectionManager(this);
            //serverLobby.connectedUsers.Add("MathPelo");
            //serverLobby.connectedUsers.Add("AnthoRicher");
            //serverLobby.connectedUsers.Add("AnthoRicher2");
            //serverLobby.ipAddress = serverConnectionManager.ipAddr;
        }

        public void ExecuteServer()
        {
            serverConnectionManager.AcceptConnection();
        }

    }
}
