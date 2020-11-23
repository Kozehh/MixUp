using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClassLibrary.Models;
using MixUp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPage : ContentPage, INotifyPropertyChanged
    {
        // Variables de classe
        private Session session;
        public event PropertyChangedEventHandler PropertyChanged;
        private PlaylistService _playlistService;
        private User User;
        private Playlist _playlist;
        private PlaylistSong<Song> selectedSong;
        public static ObservableCollection<PlaylistSong<Song>> songList;
        public static PagingObject<PlaylistSong<Song>> playlistSongs;
        public const string songToAdd = "/cAddSong:";

        // Variables qui modifient propriétés du UI quand elles changent
        public Playlist Playlist
        {
            get { return _playlist;}
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PlaylistSong<Song>> SongList
        {
            get { return songList; }
            set
            {
                songList = value;
                OnPropertyChanged();
            }
        }

        public PlaylistSong<Song> SelectedSong
        {
            get { return selectedSong; }
            set
            {
                selectedSong = value;
                OnPropertyChanged();
            }
        }

        // Constructeur de la page
        public MusicPage(User user, Session lobbySession)
        {
            InitializeComponent();
            BindingContext = this;
            session = lobbySession;
            User = user;
            ShowUserPlaylists();
        }


        // Méthodes
        private async void ShowUserPlaylists()
        {
            List<Playlist> playlists = new List<Playlist>();
            _playlistService = new PlaylistService();
            var userPlaylists = await _playlistService.GetPlaylists(User.Token);
            foreach (var playlist in userPlaylists.Items)
            {
                playlists.Add(playlist);
            }

            Playlist = playlists[0];


            playlistSongs = await _playlistService.GetPlaylistSongs(User.Token, Playlist.Id);
            SongList = new ObservableCollection<PlaylistSong<Song>>(playlistSongs.Items);
        }


        // Événements 
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchResults.ItemsSource = new ObservableCollection<PlaylistSong<Song>>(SearchBarHandler.GetSearchResults(e.NewTextValue));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
        // Commandes
        public ICommand AddedCommand => new Command(AddSongToList);

        private void AddSongToList()
        {
            if (selectedSong != null)
            {
                session.SendMessage(songToAdd + selectedSong.Song.Id);
            }
        }
        
    }
}