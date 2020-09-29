using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
                UpdateLobby();
            }
            
        }

        public void UpdateLobby()
        {
            SendMessage(ref socket, "Test Server!!!!!!!!!!!!!!!!!!");
            //foreach (ServerThread st in lobbyServer.serverThreads)
            //{
                
            //}
            
        }

        private void SendMessage(ref Socket socket, String messageToServer)
        {
            byte[] message = Encoding.ASCII.GetBytes(messageToServer);
            // Send a message to Client using Send() method 
            socket.Send(message);
        }
    } 
}
