using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using ClassLibrary.Models;

namespace MixUp.Services
{
    public class MediaPlayerService
    {
        private const string addToQueue = "https://api.spotify.com/v1/me/player/queue";
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
    }
}