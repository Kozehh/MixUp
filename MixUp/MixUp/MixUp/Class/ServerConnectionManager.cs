using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MixUp
{
    public class ServerConnectionManager
    {
        private IPHostEntry ipHost;
        private IPAddress ipAddr;
        private IPEndPoint localEndPoint;
        private Socket listener;
        private Server server;

        public ServerConnectionManager(Server server)
        {
            this.server = server;
            // Establish the local endpoint for the socket. 
            // Dns.GetHostName returns the name of the host running the application. 
            ipHost = Dns.GetHostEntry(Dns.GetHostName());

            localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            // Creation TCP/IP Socket using Socket Class Costructor
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void AcceptConnection()
        {
            try
            { 
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");
                    try
                    {
                        // Suspend while waiting for  incoming connection Using Accept() method the server will accept connection of client 
                        Socket clientSocket = listener.Accept();
                        Console.WriteLine("Server accepted!!!");
                        ServerThread st = new ServerThread(clientSocket, server);
                        Thread serverThread = new Thread(st.ExecuteServerThread);
                        serverThread.Start();
                        server.serverThreads.Add(serverThread);
                        server.connectedUsersList.Add(clientSocket);
                    }
                    catch (Exception excep)
                    {
                        Console.WriteLine("Server accept error...");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
