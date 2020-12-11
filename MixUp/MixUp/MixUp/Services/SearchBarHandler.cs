using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ClassLibrary.Models;
using MixUp.Pages;
using Xamarin.Forms;

namespace MixUp.Services
{
    public class SearchBarHandler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SearchBarHandler()
        {
        }

        public static List<PlaylistSong<Song>> GetSearchResults(string query = null)
        {
            var normalizeQuery = query.ToLower();
            if (normalizeQuery == string.Empty)
            {
                return MusicPage.allSongs.OrderBy(item => item.Song.Name).ToList();
            }
            return MusicPage.allSongs.Where(item => item.Song.Name.ToLower().Contains(normalizeQuery)).ToList();/*
                                                        &&
                                                        item.Song.Artists.Any(artist => artist.Name.ToLower().Contains(normalizeQuery)))*/
            
        }
    }
}