using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ClassLibrary.Models;
using MixUp.Pages;
//using MixUp.Views;
using Xamarin.Forms;

namespace MixUp.Services
{
    public class SearchBarHandler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SearchBarHandler()
        {
        }

        public static List<PlaylistSong<Song>> GetSearchResults(string query)
        {
            var normalizeQuery = query?.ToLower() ?? "";
            if (normalizeQuery.Equals(""))
            {
                MusicPage.songList = new ObservableCollection<PlaylistSong<Song>>(MusicPage.playlistSongs.Items);
                return MusicPage.songList.OrderBy(item => item.Song.Name).ToList<PlaylistSong<Song>>();
            }
            else
            {
                return MusicPage.songList.Where(item => item.Song.Name.ToLower().Contains(normalizeQuery)).OrderBy(
                    item => item.Song.Name).ToList<PlaylistSong<Song>>();/*
                                                        &&
                                                        item.Song.Artists.Any(artist => artist.Name.ToLower().Contains(normalizeQuery)))*/
            }
        }


        /*
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = MusicPage.songList.OrderBy(item => item.Song.Name);
            }
            else
            {
                ItemsSource = MusicPage.songList.Where(item => item.Song.Name.ToLower().Contains(newValue.ToLower()) 
                                                               &&
                                                               item.Song.Artists.Any(artist => artist.Name.ToLower().Contains(newValue.ToLower())))
                    .OrderBy(
                        item => item.Song.Name).ToList<PlaylistSong<Song>>();
            }
        }*/
    }
}