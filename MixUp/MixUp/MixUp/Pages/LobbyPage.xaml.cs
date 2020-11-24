using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
    public partial class LobbyPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Session session;
        public string ip;
        private Thread clientThread;
        private Thread serverThread;
        private Server server;
        private User HostUser;
        public const string songToAdd = "/cAddSong:";
        public Lobby lobbyPagelobby;
        public static ObservableCollection<Song> songList;
        private MediaPlayerService playerService;

        public LobbyPage(string name, string ip, Thread st, Server server, User user)
        {
            InitializeComponent();
            SongList = new ObservableCollection<Song>();
            playerService = new MediaPlayerService();
            lobbyIp.Text = "TESTTING";
            this.server = server;
            serverThread = st;
            HostUser = user;
            this.ip = ip;
            // Start the Session thread
            Session lobbySession = new Session(name, this);
            session = lobbySession;
            ThreadStart clientWork = lobbySession.ExecuteClient;
            clientThread = new Thread(clientWork);
            clientThread.Start();
            BindingContext = this;
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

        public void Update(Lobby lobby)
        {
            lobbyPagelobby = lobby;
            foreach (Song song in lobby.songList)
            {
                if (!SongList.Contains(song))
                {
                    playerService.AddToQueue(song, HostUser.Token);
                }
            }
            SongList = new ObservableCollection<Song>(lobby.songList);
            Device.BeginInvokeOnMainThread(() =>
            {
                lobbyIp.Text = lobby.ipAddress.ToString();
            });
        }

        void OnSendButtonClicked(object sender, EventArgs args)
        {
            //String messageToSend = messageEntry.Text;
            //session.SendMessage(messageToSend);
        }

        async void OnMusicPageClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MusicPage(HostUser, session));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<Song> SongList
        {
            get { return songList; }
            set
            {
                songList = value;
                
                OnPropertyChanged();
            }
        }
    }
}