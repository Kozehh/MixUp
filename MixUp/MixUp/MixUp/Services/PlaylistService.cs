using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClassLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MixUp.Services
{
    public class PlaylistService
    {
        private const string _playlistURL = "https://api.spotify.com/v1/me/playlists";
        private const string _playlistTracksURL = "https://api.spotify.com/v1/playlists/playlist_id/tracks";
        private JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private HttpClient client;

        public PlaylistService(Token token)
        { 
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        }

        // Get a list of current user's playlists
        public async Task<PagingObject<Playlist>> GetPlaylists(Token token)
        {
            var res = await client.GetAsync(_playlistURL);
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagingObject<Playlist>>(json, Settings);
        }

        public async Task<PagingObject<PlaylistSong<Song>>> GetPlaylistSongs(Token token, string id)
        {
            try
            {
                var url = _playlistTracksURL.Replace("playlist_id", id);
                var res = await client.GetAsync(url);
                var json = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PagingObject<PlaylistSong<Song>>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
    }
}