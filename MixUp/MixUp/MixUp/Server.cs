using System;
using System.Collections.Generic;
using System.Text;

// A C# Program for Server 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MixUp
{
    class Server
    {
        public Lobby serverLobby;

        public List<ServerThread> serverThreads;

        public List<Socket> connectedUsersList;

        public ServerConnectionManager serverConnectionManager;
        public Server()
        {
            serverLobby = new Lobby();
            serverThreads = new List<ServerThread>();
            connectedUsersList = new List<Socket>();
            serverConnectionManager = new ServerConnectionManager(this);
        }

        public void ExecuteServer()
        {
            serverConnectionManager.AcceptConnection();
        }

    }
}
