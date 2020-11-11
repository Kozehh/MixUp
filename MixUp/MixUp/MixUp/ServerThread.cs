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

        public void CommandInterpreter(String commandLine)
        {
            string command = commandLine.Substring(0, commandLine.IndexOf(":"));
            string parameters = commandLine.Substring(commandLine.IndexOf(":") + 1);
            switch (command)
            {
                case "Join":
                    //server.serverLobby.connectedUsers.Add(parameters);
                    return;

                default:
                    return;
            }
        }

    }

}
