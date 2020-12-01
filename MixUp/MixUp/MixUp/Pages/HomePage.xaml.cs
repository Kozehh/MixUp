using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ClassLibrary.Models;
using MixUp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly User _user;
        public HomePage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        async void OnHostButtonClicked(object sender, EventArgs args)
        {
            _user.UserPlaylists = await GetUserPlaylists(_user);
            await Navigation.PushAsync(new HostPage(_user));
        }

        async void OnJoinButtonClicked(object sender, EventArgs args)
        {
            _user.UserPlaylists = await GetUserPlaylists(_user);
            await Navigation.PushAsync(new JoinPage(_user));
        }

        private async Task<List<Playlist>> GetUserPlaylists(User user)
        {
            List<Playlist> playlists = new List<Playlist>();
            PlaylistService _playlistService = new PlaylistService(user.Token);
            var userPlaylists = await _playlistService.GetPlaylists(user.Token);
            foreach (var playlist in userPlaylists.Items)
            {
                playlist.PlaylistSongs = await _playlistService.GetPlaylistSongs(user.Token, playlist.Id);
                playlists.Add(playlist);
            }

            return playlists;
        }

    }
}