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
        }

        public void ExecuteServer()
        {
            serverConnectionManager.AcceptConnection();
        }

    }
}
