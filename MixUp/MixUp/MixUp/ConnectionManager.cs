using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Xamarin.Forms.Internals;


namespace MixUp
{
    public class ConnectionManager
    {
        public Session session;
        public Socket socket;
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint localEndPoint;
        Socket sender;
        Int32 port = 11000;
        public ConnectionManager(Session session)
        {
            this.session = session;
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddr, port);
            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToServer()
        {
            try
            {
                sender.Connect(localEndPoint);
                this.socket = sender;
                Console.WriteLine("connected to -> {0} ", sender.RemoteEndPoint.ToString());

                // receiving message loop
                while (true)
                {
                    Console.WriteLine("client receive blockk...");
                    byte[] messageReceived = new byte[1024];
                    int byteRecv = sender.Receive(messageReceived);
                    session.UpdateLobby(messageReceived);
                    Console.WriteLine("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                }
            }

            // Manage of Socket's Exceptions 
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public void DisconnectFromServer()
        {
            // Close Socket using the method Close() 
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }



    }
}
