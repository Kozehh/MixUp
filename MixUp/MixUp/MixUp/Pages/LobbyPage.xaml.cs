using ClassLibrary.Models;
using MixUp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LobbyPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public const string songToAddCommand = "/cAddSong:";
        public static ObservableCollection<Song> songList;
        public String ip;

        private Lobby lobbyPagelobby;
        private MediaPlayerService playerService;
        private Thread clientThread;
        private Thread serverThread;
        private Server server;
        private User HostUser;
        private Session session;

        public LobbyPage(string name, string ip, Thread st, Server server, User user)
        {
            InitializeComponent();
            RefreshCommand = new Command(ExecuteRefreshCommand);
            SongList = new ObservableCollection<Song>();
            playerService = new MediaPlayerService();

            this.HostUser = user;
            HostUser.Devices = playerService.GetUserDevices(HostUser);
            this.server = server;
            this.serverThread = st;
            this.ip = ip;

            // Start the Session thread
            Session lobbySession = new Session(name, this);
            session = lobbySession;
            ThreadStart clientWork = lobbySession.ExecuteClient;
            clientThread = new Thread(clientWork);
            clientThread.Start();
            BindingContext = this;
            
            // Trigger initial refresh for the page
            session.SendMessage("refresh");
        }

        public void Update(Lobby lobby)
        {
            // Visually Update the lobby
            lobbyPagelobby = lobby;
            SongList = new ObservableCollection<Song>(lobby.songList);
            Device.BeginInvokeOnMainThread(() =>
            {
                lobbyName.Text = lobby.name.ToString();
                lobbyIp.Text = lobby.ipAddress.ToString();
            });
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

        public ICommand RefreshCommand { get; }

        bool isRefreshing;
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


        /*async void OnDisconnectButtonClicked(object sender, EventArgs args)
        {
            if (serverThread != null)
            {
                foreach (Thread t in server.serverThreads)
                {
                    t.Abort();
                }
                serverThread.Abort();
            }
            clientThread.Abort();
            await Navigation.PopAsync();
        }*/
    }
}