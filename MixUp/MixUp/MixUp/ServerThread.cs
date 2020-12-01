using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using ClassLibrary.Models;
using Java.Lang;
using Java.Interop;
using MixUp.Services;
using Byte = System.Byte;
using String = System.String;

namespace MixUp
{
    [BroadcastReceiver]
    public class ServerThread : BroadcastReceiver
    {
        protected Socket socket;
        public Server server;
        public static Song currentPlayingSong = null;
        public static Song nextInQueue = null;
        public static MediaPlayerService playerService;
        public static bool notPlayed;
        public static List<Song> queueList;

        public ServerThread()
        {
        }

        public ServerThread(Socket clientSocket, Server lobbyServer)
        {
            this.socket = clientSocket;
            this.server = lobbyServer;
            playerService = new MediaPlayerService(server._userHost);
            queueList = new List<Song>();
        }

        public void ExecuteServerThread()
        {
            byte[] bytes = new Byte[1024];
            // server receiving loop 
            while (true)
            {
                string data = null;
                Console.WriteLine("server thread receive block...");
                int numByte = socket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, numByte);
                Console.WriteLine("Text received -> {0} ", data);

                // 1. INTERPRETER LE MESSAGE/COMMANDE RECU
                if (data.Length > 1)
                {
                    if (data.Substring(0, 2) == "/c")
                    {
                        CommandInterpreterAsync(data.Substring(2));
                    }
                }
                // 2. UPDATE LE LOBBY TOUT LE TEMPS 
                UpdateLobby();
            }
            
        }

        public void UpdateLobby()
        {
            // 1. Serialiser le lobby.
            // SerializePath
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            folder += "/LobbyData.dat";

            Stream stream = File.Open(folder, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, server.serverLobby);
            stream.Close();

            stream = File.Open(folder, FileMode.Open);
            byte[] message = ReadFully(stream);
            stream.Close();

            // 2. envoyer le lobby par message.
            foreach (Socket sock in server.connectedUsersList)
            {
                SendMessage(sock, message);
            }

            
        }

        private void SendMessage(Socket socket, byte[] message)
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

        public void CommandInterpreterAsync(String commandLine)
        {
            string command = commandLine.Substring(0, commandLine.IndexOf(":"));
            string parameters = commandLine.Substring(commandLine.IndexOf(":") + 1);
            server.serverLobby.songList = queueList;
            switch (command)
            {
                case "AddSong":
                    SongService service = new SongService();
                    Song song = service.GetSongById(server._userHost.Token, parameters).Result;
                    server.serverLobby.songList.Add(song);
                    if (currentPlayingSong == null)
                    {
                        currentPlayingSong = song;
                        notPlayed = true;
                        server.serverLobby.songList.Remove(song);
                    }
                    if (nextInQueue == null && server.serverLobby.songList.Count > 0)
                    {
                        nextInQueue = server.serverLobby.songList[0];
                    }

                    queueList = server.serverLobby.songList;
                    return;

                default:
                    return;
            }
        }

        // Handle the alarm manager event
        // Used for adding the next song in queue when the current playing song is finishing
        public override void OnReceive(Context context, Intent intent)
        {
            intent.Extras.Clear();
            var alarmIntent = new Intent(context, typeof(ServerThread));
            AlarmManager alarmManager = (AlarmManager)context.ApplicationContext.GetSystemService(Context.AlarmService);
            var currentTime = Calendar.GetInstance(Android.Icu.Util.TimeZone.Default).TimeInMillis;

            if (currentPlayingSong == null)
            {
                var pending = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
                alarmManager.Set(AlarmType.Rtc, currentTime + (long)TimeSpan.FromSeconds(2).TotalMilliseconds, pending);
            }
            else
            {
                if (nextInQueue != null)
                {
                    playerService.AddToQueue(nextInQueue);
                    currentPlayingSong = nextInQueue;
                    queueList.Remove(currentPlayingSong);
                    if (queueList.Count > 0)
                    {
                        nextInQueue = queueList[0];
                    }
                    else
                    {
                        nextInQueue = null;
                    }
                    var pending = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.CancelCurrent);
                    alarmManager.Cancel(pending);
                    alarmManager.SetExact(AlarmType.Rtc, (currentTime + currentPlayingSong.DurationMs) - 10000, pending);
                    
                }
                else if (notPlayed)
                {
                    playerService.PlaySong(currentPlayingSong);
                    var pending = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.CancelCurrent);
                    alarmManager.Cancel(pending);
                    alarmManager.SetExact(AlarmType.Rtc, (currentTime + currentPlayingSong.DurationMs), pending);
                    notPlayed = false;
                }
                else
                {
                    currentPlayingSong = null;
                }
            }
        }
    }

}