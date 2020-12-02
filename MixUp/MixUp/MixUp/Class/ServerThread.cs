using ClassLibrary.Models;
using MixUp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using Android.App;
using Android.Content;
using Android.Icu.Util;

namespace MixUp
{
    [BroadcastReceiver]
    public class ServerThread : BroadcastReceiver
    {
        public Socket socket;
        public static Server server = null;
        public static MediaPlayerService playerService;
        
        public ServerThread()
        {
        }
        
        public ServerThread(Socket clientSocket, Server lobbyServer)
        {
            socket = clientSocket;
            server = lobbyServer;
            playerService = new MediaPlayerService(server._userHost);
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

                // Command Interpreter
                if (data.Length > 1)
                {
                    if (data.Substring(0, 2) == "/c")
                    {
                        CommandInterpreterAsync(data.Substring(2));
                    }
                }
                // 2. Always Update Lobby
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

        public void CommandInterpreterAsync(string commandLine)
        {
            string command = commandLine.Substring(0, commandLine.IndexOf(":"));
            string parameters = commandLine.Substring(commandLine.IndexOf(":") + 1);
            switch (command)
            {
                case "AddSong":
                    SongService service = new SongService();
                    Song song = service.GetSongById(server._userHost.Token, parameters).Result;
                    server.serverLobby.songList.Add(song);
                    if (server.serverLobby.currentPlayingSong == null)
                    {
                        server.serverLobby.currentPlayingSong = song;
                        server.serverLobby.notPlayed = true;
                        server.serverLobby.songList.Remove(song);
                    }
                    if (server.serverLobby.nextInQueue == null && server.serverLobby.songList.Count > 0)
                    {
                        server.serverLobby.nextInQueue = server.serverLobby.songList[0];
                    }

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

            if (server == null || server.serverLobby.currentPlayingSong == null)
            {
                var pending = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
                alarmManager.Set(AlarmType.Rtc, currentTime + (long)TimeSpan.FromSeconds(2).TotalMilliseconds, pending);
            }
            else
            {
                if (server != null && server.serverLobby.nextInQueue != null)
                {
                    playerService.AddToQueue(server.serverLobby.nextInQueue);
                    server.serverLobby.currentPlayingSong = server.serverLobby.nextInQueue;
                    server.serverLobby.songList.Remove(server.serverLobby.currentPlayingSong);
                    if (server.serverLobby.songList.Count > 0)
                    {
                        server.serverLobby.nextInQueue = server.serverLobby.songList[0];
                    }
                    else
                    {
                        server.serverLobby.nextInQueue = null;
                    }
                    var pending = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.CancelCurrent);
                    alarmManager.Cancel(pending);
                    alarmManager.SetExact(AlarmType.Rtc, (currentTime + server.serverLobby.currentPlayingSong.DurationMs), pending);

                }
                else if (server.serverLobby.notPlayed)
                {
                    playerService.PlaySong(server.serverLobby.currentPlayingSong);
                    var pending = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.CancelCurrent);
                    alarmManager.Cancel(pending);
                    alarmManager.SetExact(AlarmType.Rtc, (currentTime + server.serverLobby.currentPlayingSong.DurationMs -10000), pending);
                    server.serverLobby.notPlayed = false;
                }
                else
                {
                    server.serverLobby.currentPlayingSong = null;
                }
                UpdateLobby();
            }
            
        }
    }

}