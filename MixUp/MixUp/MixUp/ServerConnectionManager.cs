using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MixUp
{
    class ServerConnectionManager
    {
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint localEndPoint;
        Socket listener;
        Server server;

        public ServerConnectionManager(Server server)
        {
            this.server = server;
            // Establish the local endpoint for the socket. 
            // Dns.GetHostName returns the name of the host running the application. 
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[1];
            localEndPoint = new IPEndPoint(ipAddr, 13000);

            // Creation TCP/IP Socket using Socket Class Costructor 
            listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        }

        public void AcceptConnection()
        {

            try
            {
                // Using Bind() method we associate a network address to the Server Socket 
                // All client that will connect to this Server Socket must know this network Address 
                listener.Bind(localEndPoint);

                // Using Listen() method we create the Client list that will want 
                // to connect to Server 
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
                        server.connectedUsersList.Add(clientSocket);
                    }
                    catch (Exception excep)
                    {
                        Console.WriteLine("Server accept error...");
                    }

                }
                /*

                // Data buffer 

                // Close client Socket using the Close() method. After closing, 
                // we can use the closed Socket for a new Client Connection 
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
