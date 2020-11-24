using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MixUpAPI.Controllers
{
    [ApiController]
    [Route("mixup")]
    public class SpotifyController : ControllerBase
    {
        private const string _authURL = "https://accounts.spotify.com/authorize?";
        private const string _tokenURL = "https://accounts.spotify.com/api/token";
        private const string _userURL = "https://api.spotify.com/v1/me";

        public string client_secret = "e86971bae67043eaa474a084eab7b356";
        public string client_id = "d8235676727f4a1b9938a49627c86640";
        public string response_type = "code";
        public string redirect_uri = "http://10.44.88.242:80/mixup/callback";
        private string _state = "profile activity";
        public string scope = "user-read-private user-read-email user-modify-playback-state user-read-playback-state";
        private string _code; // Code received from authorize access -> Will be exchange for an access
        

        private string dbManagerApi = Environment.GetEnvironmentVariable("DB_MANAGER_ADDR");


        [HttpGet]
        [Route("auth-finished")]
        public bool AuhentificationFinished()
        {
            return Program.authentificationFinished;
        }

        [HttpGet]
        [Route("callback")]
        public string Callback([FromQuery] string code, [FromQuery] string state, [FromQuery] string error)
        {
            if (error != null)
            {
                // TODO : Show error
                Console.WriteLine(error);
                return null;
            }
            else if (state == null || _state != state)
            {
                // TODO : Error: State mismatch
                Console.WriteLine("Error: State mismatch");
                return null;
            }
            else
            {
                return code;
            }
        }

        [HttpGet]
        [Route("authenticate")]
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

        [HttpPost]
        [Route("requestToken")]
        public string RequestToken([FromBody] string code)
        {
            string s = "authorization_code";
            var param = new Dictionary<string, string>()
            {
                {"code", code },
                {"redirect_uri", redirect_uri},
                {"grant_type", s},
                {"client_id", client_id},
                {"client_secret", client_secret}
            };

            var token = GetNewToken(param);
            // Associate the member with his token in the db
            Token ll = PostDbManager("Token/Add", token);

            var json = JsonConvert.SerializeObject(ll);

            return json;
        }

        [HttpPost]
        [Route("refreshToken")]
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

        [HttpPost]
        [Route("user")]
        public User GetUser([FromBody] Token token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                
                var res = client.GetAsync(_userURL).Result;
                var userJson = res.Content.ReadAsStringAsync().Result;
                
                return JsonConvert.DeserializeObject<User>(userJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
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


        public Token PostDbManager(string apiPath, Token dataToSend)
        {
            HttpClient client = new HttpClient();
            try
            {
                var serialize = JsonConvert.SerializeObject(dataToSend);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var result = client.PostAsync(dbManagerApi + "/db-manager/" + apiPath, toSend).Result;
                return JsonConvert.DeserializeObject<Token>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        
        

    }
}
