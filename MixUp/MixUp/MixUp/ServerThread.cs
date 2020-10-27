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
            // SerializePath
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            folder += "/LobbyData.dat";

            Stream stream = File.Open(folder, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, server.serverLobby);
            stream.Close();





            Console.WriteLine(server.serverLobby.connectedUsers[0]);



            stream = File.Open(folder, FileMode.Open);
            byte[] message = ReadFully(stream);

            // 2. envoyer le lobby par message.
            SendMessage(ref socket, message);


        }

        private void SendMessage(ref Socket socket, byte[] message)
        { 

            // Send a message to Client using Send() method 
            socket.Send(message);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }

}


/*
            
            bf = new BinaryFormatter();
            server.serverLobby = (Lobby)bf.Deserialize(stream);
            stream.Close();
*/
