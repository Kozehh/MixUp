using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Android.App;
using Android.Content;
using Android.Icu.Util;
using MixUp.Pages;

namespace MixUp
{
    public class Session
    {
        public Lobby lobby;
        public ConnectionManager connectionManager;
        public LobbyPage sessionLobbyPage;

        public Session(string name, LobbyPage lobbyPage)
        {
            sessionLobbyPage = lobbyPage;
            connectionManager = new ConnectionManager(this);
            lobby = new Lobby(null, null, null);
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
            sessionLobbyPage.Update(lobby);
        }

        public bool SendMessage(string message)
        {
            byte[] messageSent = Encoding.ASCII.GetBytes(message);
            if (connectionManager.socket != null)
            {
                int byteSent = connectionManager.socket.Send(messageSent);
                return true;
            }
            return false;
        }
    }
}
