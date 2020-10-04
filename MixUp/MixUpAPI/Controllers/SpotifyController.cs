using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MixUpAPI.Controllers
{
    [ApiController]
    [Route("MixUpApi")]
    public class SpotifyController : ControllerBase
    {
        private const string _authURL = "https://accounts.spotify.com/authorize?";
        private const string _tokenURL = @"https://accounts.spotify.com/api/token";
        private const string _playlistURL = @"https://api.spotify.com/v1/me/playlists";
        private HttpClient _client = new HttpClient();

        public string client_secret = "e86971bae67043eaa474a084eab7b356";
        public string client_id = "d8235676727f4a1b9938a49627c86640";
        public string response_type = "code";
        public string redirect_uri = "http://10.0.2.2:5000/MixUpApi/callback";
        private string _state = "profile activity";
        public string scope = "user-read-private user-read-email";
        private string _code; // Code received from authorize access -> Will be exchange for an access
        public static AuthorizationRequest reeqq = new AuthorizationRequest();
        

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
                reeqq.code = code;
                GetToken();
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
                {"response_type", response_type },
                {"redirect_uri", redirect_uri},
                {"state", _state},
                {"scope", scope}
            };
            var newUrl = QueryHelpers.AddQueryString(_authURL, param);

            return newUrl;
        }

        [HttpGet]
        [Route("Token")]
        public async void GetToken()
        {
            string s = "authorization_code";
            var param = new Dictionary<string, string>()
            {
                {"code", reeqq.code },
                {"redirect_uri", redirect_uri},
                {"grant_type", s},
                {"client_id", client_id},
                {"client_secret", client_secret}
            };

            HttpClient c = new HttpClient();
            HttpResponseMessage tknRes = await c.PostAsync(_tokenURL, new FormUrlEncodedContent(param));
            var jsonContent = await tknRes.Content.ReadAsStringAsync();
            Token tok = JsonConvert.DeserializeObject<Token>(jsonContent);

            GetPlaylists(tok);
            Console.WriteLine("OK");
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
    internal class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
