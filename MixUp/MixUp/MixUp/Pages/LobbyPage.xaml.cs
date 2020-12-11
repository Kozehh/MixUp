using ClassLibrary.Models;
using MixUp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
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
        public String ip;

        private Thread clientThread;
        private Thread serverThread;
        private Server server;
        private User user;
        private const string roomCodePH = "Room Code is : ";
        public const string songToAdd = "/cAddSong:";
        public static ObservableCollection<Song> songList;
        public static Session session;
        private MusicPage playlists;
        private ObservableCollection<Playlist> _playlists;
        private Playlist selectedPlaylist;
        private Song currentlyPlaying;

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
                // Get info of the Host playback
                if (server != null && st != null)
                {
                    this.server = server;
                    serverThread = st;
                }

                currentlyPlaying = null;
                this.ip = ip;
                
                session = new Session(name, this);
                // Start the Session thread
                ThreadStart clientWork = session.ExecuteClient;
                clientThread = new Thread(clientWork);
                clientThread.Start();
                ExecuteRefreshCommand();
                BindingContext = this;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void Update(Lobby lobby)
        {
            // Visually Update the lobby
            session.lobby = lobby;
            if (session.lobby.currentPlayingSong != null)
            {
                CurrentlyPlaying = session.lobby.currentPlayingSong;
            }
            SongList = new ObservableCollection<Song>(lobby.songList);
            Device.BeginInvokeOnMainThread(() =>
            {
                lobbyName.Text = lobby.name;
                lobbyCode.Text = roomCodePH + lobby.roomCode;
            });
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

        public void ExecuteRefreshCommand()
        {
            session.SendMessage("/crefresh:");
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
                await Navigation.PushAsync(new MusicPage(session, selectedPlaylist));
            }
        }
    }
}