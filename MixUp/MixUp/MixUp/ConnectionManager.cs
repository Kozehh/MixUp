using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
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
        Int32 port = 13000;
        public ConnectionManager(Session session)
        {
            this.session = session;
            // Establish the remote endpoint for the socket. This example uses port 13000 on the local computer. 
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[1];

            if (!session.isAdmin)
            {
                ipAddr = IPAddress.Parse("192.168.200.2");
                port = 13000;
            }
            //
            localEndPoint = new IPEndPoint(ipAddr, port);
            // Creation TCP/IP Socket using Socket Class Costructor 
            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToServer()
        {

            try
            {
                // Connect Socket to the remote endpoint using method Connect() 
                sender.Connect(localEndPoint);
                this.socket = sender;
                // We print EndPoint information that we are connected 
                Console.WriteLine("connected to -> {0} ", sender.RemoteEndPoint.ToString());

                // receiving message loop
                while (true)
                {
                    Console.WriteLine("client receive blockk...");
                    byte[] messageReceived = new byte[1024];
                    int byteRecv = sender.Receive(messageReceived);
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


        public void SendMessage(String message)
        {
            //byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
            byte[] messageSent = Encoding.ASCII.GetBytes(message);
            int byteSent = this.socket.Send(messageSent);

            // Data buffer 
            byte[] messageReceived = new byte[1024];

            // We receive the messagge using the method Receive(). This method returns number of bytes 
            // received, that we'll use to convert them to string 
            //int byteRecv = socket.Receive(messageReceived);
            //Console.WriteLine("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
        }
    }
}
