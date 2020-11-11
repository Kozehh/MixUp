using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Models;
using MixUp.Services;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LobbyPage : ContentPage
    {
        Session session;
        public string ip;
        public string name;
        private List<Song> Queue;
        private Thread clientThread;
        private Thread serverThread;
        private Server server;
        private User HostUser;
        private PlaylistService PlaylistService;

        private JsonSerializer Settings = new JsonSerializer()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public LobbyPage(string name, string ip, Thread st, Server server, User user)
        {
            InitializeComponent();
            this.server = server;
            this.serverThread = st;
            HostUser = user;
            this.ip = ip;
            this.name = name;

            // Start the Session thread
            Session lobbySession = new Session(name, this);
            this.session = lobbySession;
            ThreadStart clientWork = lobbySession.ExecuteClient;
            clientThread = new Thread(clientWork);
            clientThread.Start();
        }
        
        async void OnDisconnectButtonClicked(object sender, EventArgs args)
        {
            if(serverThread != null)
            {
                foreach(Thread t in server.serverThreads)
                {
                    t.Abort();
                }
                serverThread.Abort();
            }
            
            clientThread.Abort();
            await Navigation.PopAsync();
        }


        void OnSendButtonClicked(object sender, EventArgs args)
        {
            String messageToSend = messageEntry.Text;
            session.SendMessage(messageToSend);
        }

        void OnAddSongButtonClicked(object sender, EventArgs args)
        {
            String songToAdd = "/cAddSong:";
            songToAdd += songEntry.Text;
            session.SendMessage(songToAdd);
        }

        async void OnAddSongClicked(object sender, EventArgs args)
        {
            List<Playlist> playlists = new List<Playlist>();
            PlaylistService = new PlaylistService();
            var playlist = await PlaylistService.GetPlaylists(HostUser.Token);
            foreach (var obj in playlist.Items)
            {
                var o = JObject.FromObject(obj);
                var x = o.ToObject<Playlist>(Settings);
                playlists.Add(x);
            }

            Song song = playlists[1].Songs[1];
            Queue.Add(song);
            Console.WriteLine(playlist.Items);
        }
    }
}