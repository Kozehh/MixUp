using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DBManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace MixUpAPI.Controllers
{
    [ApiController]
    [Route("mixup")]
    public class SpotifyController : ControllerBase
    {
        private const string _authURL = "https://accounts.spotify.com/authorize?";
        private const string _tokenURL = "https://accounts.spotify.com/api/token";
        private const string _playlistURL = "https://api.spotify.com/v1/me/playlists";
        private HttpClient _client = new HttpClient();

        public string client_secret = "e86971bae67043eaa474a084eab7b356";
        public string client_id = "d8235676727f4a1b9938a49627c86640";
        public string response_type = "code";
        public string redirect_uri = "http://10.44.88.242:80/mixup/callback";
        private string _state = "profile activity";
        public string scope = "user-read-private user-read-email";
        private string _code; // Code received from authorize access -> Will be exchange for an access

        private string _dbManagerApi = Environment.GetEnvironmentVariable("DB_MANAGER_ADDR");
        //private string _dbManagerApi = "http://localhost:9000/db-manager/";


        [HttpGet]
        [Route("callback")]
        public bool LoginFailed([FromQuery] string code, [FromQuery] string state, [FromQuery] string error)
        {
            if (error != null)
            {
                // TODO : Show error
                Console.WriteLine(error);
                return false;
            }
            else if (state == null || _state != state)
            {
                // TODO : Error: State mismatch
                return false;
            }
            else
            {
                Program.code = code;
                RequestToken();
                return true;
            }
        }

        [HttpGet]
        [Route("Authenticate")]
        public string Authenticate()
        {
            var param = new Dictionary<string, string>()
            {
                {"client_id", client_id},
                {"response_type", response_type},
                {"redirect_uri", redirect_uri},
                {"state", _state},
                {"scope", scope}
            };
            var newUrl = QueryHelpers.AddQueryString(_authURL, param);

            return newUrl;
        }

        [HttpGet]
        [Route("RequestToken")]
        public void RequestToken()
        {
            string s = "authorization_code";
            var param = new Dictionary<string, string>()
            {
                {"code", Program.code },
                {"redirect_uri", redirect_uri},
                {"grant_type", s},
                {"client_id", client_id},
                {"client_secret", client_secret}
            };

            var token = GetNewToken(param);
            Console.WriteLine("Access token" + token.AccessToken);
            // Associate the member with his token in the db
            PostDbManager("Token/Add", token);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public Token RefreshToken([FromBody] Token tokenToRefresh)
        {

            string refresh = "refresh_token";
            var param = new Dictionary<string, string>()
            {
                {"grant_type", refresh},
                {refresh, tokenToRefresh.RefreshToken}
            };

            Token tokenRefreshed = GetNewToken(param);

            // Update the users token in the db
            PostDbManager("Token/Update", tokenRefreshed);
            return tokenRefreshed;
        }

        public Token GetNewToken(Dictionary<string, string> requestBody)
        {
            HttpClient client = new HttpClient();
            var response = client.PostAsync(_tokenURL, new FormUrlEncodedContent(requestBody)).Result;
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            Token token = JsonConvert.DeserializeObject<Token>(jsonContent);
            //TODO: Catch errors and exceptions
            return token;
        }


        public void PostDbManager(string apiPath, Token dataToSend)
        {
            HttpClient client = new HttpClient();
            try
            {
                var serialize = JsonConvert.SerializeObject(dataToSend);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                Console.WriteLine("ToSend" + toSend.ReadAsStringAsync().Result);
                var result = client.PostAsync(_dbManagerApi + "/db-manager/" + apiPath, toSend).Result;
                Console.WriteLine(result.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async void GetPlaylists(Token token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var res = await client.GetAsync(_playlistURL);
            var json = await res.Content.ReadAsStringAsync();
            var xd = JsonConvert.DeserializeObject(json);
            Console.WriteLine("xd");
        }

    }
}
