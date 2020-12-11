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
        private IPAddress hostAddr;
        private IPHostEntry hostName;
        private IPEndPoint remoteEndPoint;
        private Socket sender;
        private Int32 port = 11000;
        private readonly IPAddress _androidEmulatorIp = IPAddress.Parse("192.168.232.2"); 

        public ConnectionManager(Session session)
        {
            this.session = session;
            hostName = Dns.GetHostEntry(Dns.GetHostName());
            hostAddr = hostName.AddressList[0];
            if (!string.IsNullOrEmpty(session.sessionLobbyPage.ip))
            {
                hostAddr = IPAddress.Parse(session.sessionLobbyPage.ip);
            }
            remoteEndPoint = new IPEndPoint(hostAddr, port);
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToServer()
        {
            try
            {
                sender.Connect(remoteEndPoint);
                this.socket = sender;
                Console.WriteLine("connected to -> {0} ", sender.RemoteEndPoint.ToString());

                // receiving message loop
                while (true)
                {
                    Console.WriteLine("client receive block...");
                    byte[] messageReceived = new byte[16384];
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                    session.UpdateLobby(messageReceived);
                }
            }

            // Manage of Socket's Exceptions y
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
