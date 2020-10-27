using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using System.Linq;
using System.Collections;

namespace MixUp
{
    class ServerThread
    {
        protected Socket socket;
        public Server server;

        public ServerThread(Socket clientSocket, Server lobbyServer)
        {
            this.socket = clientSocket;
            this.server = lobbyServer;
        }

        
        public void ExecuteServerThread()
        {
            byte[] bytes = new Byte[1024];

            while (true)
            {
                string data = null;
                Console.WriteLine("server thread receive block...");
                int numByte = socket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, numByte);
                Console.WriteLine("Text received -> {0} ", data);

                // 1. INTERPRETER LE MESSAGE/COMMANDE RECU
                // EST CE QU'IL S'AGIT D'UNE CHACON RAJOUTER, ...
                // 2. UPDATE LE LOBBY TOUT LE TEMPS 
                UpdateLobby();
            }
            
        }

        public void UpdateLobby()
        {
            // TO DO :
            // 1. Serialiser le lobby.
            Stream stream = File.Open("LobbyData.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, server.serverLobby);
            stream.Close();


            server.serverLobby = null;

            stream = File.Open("LobbyData.dat", FileMode.Open);
            bf = new BinaryFormatter();
            server.serverLobby = (Lobby)bf.Deserialize(stream);
            stream.Close();

            Console.WriteLine(server.serverLobby);
            
            // 2. envoyer le lobby par message.
            SendMessage(ref socket, "UPDATE LOBBY");

        }

        private void SendMessage(ref Socket socket, String messageToServer)
        {
            byte[] message = Encoding.ASCII.GetBytes(messageToServer);
            // Send a message to Client using Send() method 
            socket.Send(message);
        }
    } 
}
