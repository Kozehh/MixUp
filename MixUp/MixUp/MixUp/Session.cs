using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Xamarin.Forms.Internals;
using System.Runtime.Serialization.Formatters.Binary;

namespace MixUp
{
    public class Session
    {
        public Lobby lobby;
        public ConnectionManager connectionManager;
        public MixUp.Pages.LobbyPage lobbyPage;
        public String name;

        public Session(String name, MixUp.Pages.LobbyPage lobbyPage)
        {
            this.lobbyPage = lobbyPage;
            connectionManager = new ConnectionManager(this);
            lobby = new Lobby(null);
            this.name = name;
        }

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
            //lobbyPage.Update(lobby);
            return;
        }

        public void JoinLobby(String name) 
        {
            String command = "/cJoin:";
            command += name;
            SendMessage(command);
        }

        public void SendMessage(String message)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes(message);
            if (connectionManager.socket != null)
            {
                int byteSent = this.connectionManager.socket.Send(messageSent);
            }
        }
    }
}
