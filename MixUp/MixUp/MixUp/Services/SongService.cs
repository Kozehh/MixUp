using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClassLibrary.Models;
using Newtonsoft.Json;

namespace MixUp.Services
{
    public class SongService
    {
        private const string trackById = "https://api.spotify.com/v1/tracks/id";

        public async Task<Song> GetSongById(Token token, string id)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var url = trackById.Replace("id", id);
            var res = await client.GetAsync(url);
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Song>(json);
        }
    }
}