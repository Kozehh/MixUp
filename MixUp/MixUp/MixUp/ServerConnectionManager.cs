using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MixUp
{
    public class ServerConnectionManager
    {
        IPHostEntry ipHost;
        public IPAddress ipAddr;
        IPEndPoint localEndPoint;
        Socket listener;
        Server server;

        public ServerConnectionManager(Server server)
        {
            this.server = server;
            // Establish the local endpoint for the socket. 
            // Dns.GetHostName returns the name of the host running the application. 
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddr, 11000);
            // Creation TCP/IP Socket using Socket Class Costructor 
            listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
                        ServerThread st = new ServerThread(clientSocket, this.server);
                        Thread serverThread = new Thread(new System.Threading.ThreadStart(st.ExecuteServerThread));
                        serverThread.Start();
                        server.serverThreads.Add(serverThread);
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
