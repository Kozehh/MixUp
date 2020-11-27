using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MixUp
{
    public class Session
    {
        public Lobby lobby;
        public ConnectionManager connectionManager;
        public MixUp.Pages.LobbyPage lobbyPage;

        public Session(String name, MixUp.Pages.LobbyPage lobbyPage)
        {
            this.lobbyPage = lobbyPage;
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
            lobbyPage.Update(lobby);
            return;
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
