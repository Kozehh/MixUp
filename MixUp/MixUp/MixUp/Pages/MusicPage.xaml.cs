using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using MixUp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private PlaylistService _playlistService;
        private User User;
        private Playlist _playlist;

        public Playlist Playlist
        {
            get { return _playlist;}
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<PlaylistSong<Song>> songList;
        public ObservableCollection<PlaylistSong<Song>> SongList
        {
            get { return songList; }
            set
            {
                songList = value;
                OnPropertyChanged();
            }
        }

        public MusicPage(User user)
        {
            InitializeComponent();
            BindingContext = this;
            User = user;
            ShowUserPlaylists();
        }

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


            var playlistSongs = await _playlistService.GetPlaylistSongs(User.Token, Playlist.Id);
            SongList = new ObservableCollection<PlaylistSong<Song>>(playlistSongs.Items);
        }
    }
}