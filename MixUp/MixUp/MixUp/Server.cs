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


        public List<ServerThread> serverThreads;

        public List<Socket> connectedUsersList;

        ServerConnectionManager serverConnectionManager;
        public Server()
        {
            serverThreads = new List<ServerThread>();
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
