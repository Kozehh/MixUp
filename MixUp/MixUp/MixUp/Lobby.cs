using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using Xamarin.Forms.Internals;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace MixUp
{
    [Serializable()]
    public class Lobby : ISerializable
    {
        public List<String> connectedUsers;
        public Lobby()
        {
            connectedUsers = new List<String>();
            connectedUsers.Add("MathPelo");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("connectedUsers", connectedUsers);
        }

        public Lobby(SerializationInfo info, StreamingContext context)
        {
            connectedUsers = (List<String>)info.GetValue("connectedUsers", typeof(List<String>));
        }
    }
}
