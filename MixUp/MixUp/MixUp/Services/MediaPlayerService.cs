using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using ClassLibrary.Models;
using Newtonsoft.Json;

namespace MixUp.Services
{
    public class MediaPlayerService
    {
        private const string addToQueue = "https://api.spotify.com/v1/me/player/queue";
        private const string userDevices = "https://api.spotify.com/v1/me/player/devices";
        private HttpClient client = new HttpClient();

        public async void AddToQueue(Song song, Token token)
        {
            var builder = new UriBuilder(addToQueue);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["uri"] = song.Uri;
            builder.Query = query.ToString();
            string url = builder.ToString();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var res = await client.PostAsync(url, null);
            Console.WriteLine("helloo");
        }

        public List<PlayerDevice> GetUserDevices(User user)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token.AccessToken);
            var res = client.GetAsync(userDevices).Result;
            var devices = JsonConvert.DeserializeObject<PayloadObject>(res.Content.ReadAsStringAsync().Result);
            return devices.Devices;
        }
    }
}