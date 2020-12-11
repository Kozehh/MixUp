using ClassLibrary.Models;
using MixUp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
        public static Playlist _playlist;
        private PlaylistSong<Song> selectedSong;
        public static ObservableCollection<PlaylistSong<Song>> allSongs;
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
        public MusicPage(Session lobbySession, Playlist playlist)
        {
            InitializeComponent();
            session = lobbySession;
            Playlist = playlist;
            allSongs = new ObservableCollection<PlaylistSong<Song>>(playlist.PlaylistSongs.Items);
            searchResult.ItemsSource = allSongs;
            BindingContext = this;
        }


        // Méthodes
        
        // Événements 
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchResult.ItemsSource = new ObservableCollection<PlaylistSong<Song>>(SearchBarHandler.GetSearchResults(e.NewTextValue));
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