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
using System.Runtime.Serialization.Formatters.Binary;

namespace MixUp
{
    public class Session
    {
        public Lobby lobby;
        public ConnectionManager connectionManager;
        public MixUp.Pages.LobbyPage lobbyPage;

        public Session(MixUp.Pages.LobbyPage lobbyPage)
        {
            this.lobbyPage = lobbyPage;
            connectionManager = new ConnectionManager(this);
            lobby = new Lobby();
        }

        // ExecuteClient() Method 
        public void ExecuteClient()
        {
            connectionManager.ConnectToServer();
        }

        public void UpdateLobby(byte[] byteRecv)
        {
            Stream stream = new MemoryStream(byteRecv);
            BinaryFormatter bf = new BinaryFormatter();
            lobby = (Lobby)bf.Deserialize(stream);
            stream.Close();

            lobbyPage.Update(lobby);

        }

    }
}
