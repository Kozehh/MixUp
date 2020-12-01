using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace MixUp
{
    public class Server
    {
        public Lobby serverLobby;
        public List<Thread> serverThreads;
        public List<Socket> connectedUsersList;
        public ServerConnectionManager serverConnectionManager;
        public User _userHost;
        public String roomCode;

        public Server(User user, String serverName, String code)
        {
            roomCode = code;
            _userHost = user;
            serverLobby = new Lobby(user, serverName, code);
            serverThreads = new List<Thread>();
            connectedUsersList = new List<Socket>();
            serverConnectionManager = new ServerConnectionManager(this);
        }

        public void ExecuteServer()
        {
            serverConnectionManager.AcceptConnection();
        }

    }
}
