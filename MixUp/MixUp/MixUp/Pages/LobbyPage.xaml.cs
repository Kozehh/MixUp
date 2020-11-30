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
using System.Windows.Input;
using Android.Provider;
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
        private User user;
        public const string songToAdd = "/cAddSong:";
        public Lobby lobbyPagelobby;
        public static ObservableCollection<Song> songList;
        
        public static Song currentlyPlaying;
        private MusicPage playlists;
        private ObservableCollection<Playlist> _playlists;
        private Playlist selectedPlaylist;

        public LobbyPage(string name, string ip, Thread st, Server server, User user)
        {
            try
            {
                InitializeComponent();
                Title = "Browse";
                RefreshCommand = new Command(ExecuteRefreshCommand);
                SongList = new ObservableCollection<Song>();
                this.user = user;
                Playlists = new ObservableCollection<Playlist>(user.UserPlaylists);
                //lobbyIp.Text = "TESTTING";
                // Get info of the Host playback
                if (server != null && st != null)
                {
                    this.server = server;
                    serverThread = st;
                }

                this.ip = ip;
                // Start the Session thread
                Session lobbySession = new Session(name, this);
                session = lobbySession;
                ThreadStart clientWork = lobbySession.ExecuteClient;
                clientThread = new Thread(clientWork);
                clientThread.Start();
                BindingContext = this;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
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
            if (ServerThread.currentPlayingSong != null)
            {
                CurrentlyPlaying = ServerThread.currentPlayingSong;
            }
            SongList = new ObservableCollection<Song>(lobby.songList);
            Device.BeginInvokeOnMainThread(() =>
            {
                //lobbyName.Text = lobby.name.ToString();
                //lobbyIp.Text = lobby.ipAddress.ToString();
            });
        }

        void OnSendButtonClicked(object sender, EventArgs args)
        {
            //String messageToSend = messageEntry.Text;
            //session.SendMessage(messageToSend);
        }

        async void OnMusicPageClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(playlists);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public Song CurrentlyPlaying
        {
            get { return currentlyPlaying; }
            set
            {
                currentlyPlaying = value;
                OnPropertyChanged();
            }
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

        public ObservableCollection<Playlist> Playlists
        {
            get { return _playlists; }
            set
            {
                _playlists = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        void ExecuteRefreshCommand()
        {
            session.SendMessage("refresh");
            // Stop refreshing
            IsRefreshing = false;
        }
        
        public async void OnPlaylistClicked(object sender, EventArgs args)
        {
            var button = (ImageButton) sender;
            string name = button.CommandParameter.ToString();
            var selectedPlaylist = user.UserPlaylists.Find(playlist => playlist.Name == name);
            if (selectedPlaylist != null)
            {
                await Navigation.PushAsync(new MusicPage(user, session, selectedPlaylist));
            }
        }
    }
}