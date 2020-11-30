using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        private const string startPlayback = "https://api.spotify.com/v1/me/player/play";
        private HttpClient client = new HttpClient();
        private string deviceId;
        private List<PlayerDevice> devices = new List<PlayerDevice>();

        public MediaPlayerService(User host)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", host.Token.AccessToken);
            devices = GetUserDevices();
            if (devices.Count > 0)
            {
                deviceId = devices[0].DeviceId;
            }
        }

        public async void AddToQueue(Song song)
        {
            var builder = new UriBuilder(addToQueue);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["uri"] = song.Uri;
            builder.Query = query.ToString();
            string url = builder.ToString();

            
            var res = await client.PostAsync(url, null);
        }

        // Get the user's devices that he can play music on
        public List<PlayerDevice> GetUserDevices()
        {
            var res = client.GetAsync(userDevices).Result;
            var json = res.Content.ReadAsStringAsync().Result;
            var devices = JsonConvert.DeserializeObject<PayloadObject>(json);
            return devices.Devices;
        }

        // Play a song on the user spotify's playback
        public void PlaySong(Song song)
        {
            var builder = new UriBuilder(startPlayback);
            builder.Port = -1;
            if (deviceId != null)
            {
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["device_id"] = deviceId;
                builder.Query = query.ToString();
            }
            string url = builder.ToString();

            var body = new Dictionary<string, List<string>>()
            {
                {"uris", 
                    new List<string>()
                    {
                        song.Uri
                    }
                }
            };
            var serialize = JsonConvert.SerializeObject(body);
            var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
            var result = client.PutAsync(url, toSend).Result;
        }
    }
}