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
    public class Server
    {
        public Lobby serverLobby;

        public List<Thread> serverThreads;

        public List<Socket> connectedUsersList;

        public ServerConnectionManager serverConnectionManager;

        public String hostName;

        public Server(String hostName)
        {
            this.hostName = hostName;
            serverLobby = new Lobby(hostName);
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
