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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MixUpAPI.Controllers
{
    [ApiController]
    [Route("mixup")]
    public class SpotifyController : ControllerBase
    {
        // URLs and URI
        private const string _authURL = "https://accounts.spotify.com/authorize?";
        private const string _tokenURL = "https://accounts.spotify.com/api/token";
        private const string _userURL = "https://api.spotify.com/v1/me";
        private string redirect_uri = "https://servermixupudes.loca.lt/mixup/callback";

        // Identifiant et code secret utilisé dans le authorization code flow pour
        // avoir un token d'accès de l'application
        private const string client_secret = "e86971bae67043eaa474a084eab7b356";
        private const string client_id = "d8235676727f4a1b9938a49627c86640";

        // Constantes utilisés pour le authorization code flow
        private const string response_type = "code";
        private const string _state = "profile activity";
        private const string scope = "user-read-private user-read-email user-modify-playback-state user-read-playback-state";
        private string _code; // Code received from authorize access -> Will be exchange for an access
        private const string auth_code = "authorization_code";

        // Variables d'environnement venant du Dockerfile
        private string dbManagerApi = Environment.GetEnvironmentVariable("DB_MANAGER_ADDR");

        private HttpClient client = new HttpClient();

        // ** Endpoints de l'API ** //

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
                Console.WriteLine(error);
                return null;
            }
            else if (state == null || _state != state)
            {
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
            var param = new Dictionary<string, string>()
            {
                {"code", code },
                {"redirect_uri", redirect_uri},
                {"grant_type", auth_code},
                {"client_id", client_id},
                {"client_secret", client_secret}
            };

            var token = GetNewToken(param);
            // Associate the member with his token in the db
            //Token ll = PostDbManager("Token/Add", token);
            var json = JsonConvert.SerializeObject(token);

            return json;
        }

        // Le token pour communiquer avec le API de Spotify est valide pour 1 heure
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
            return null;
        }

        [HttpPost]
        [Route("user")]
        public User GetUser([FromBody] Token token)
        {
            try
            {
                // On ajoute a l'entête de la requête, le token d'access
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var res = client.GetAsync(_userURL).Result;
                var userJson = res.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<User>(userJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }

        [HttpPost]
        [Route("lobby/create")]
        public bool SaveLobbyCode([FromBody] LobbyInfo lobbyInfo)
        {
            bool canSaveLobby = true;
            try
            {
                var serialize = JsonConvert.SerializeObject(lobbyInfo);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var res = client.PostAsync(dbManagerApi + "/dbmanager/lobby/create", toSend).Result;
                canSaveLobby = JsonConvert.DeserializeObject<bool>(res.Content.ReadAsStringAsync().Result);
                return canSaveLobby;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return canSaveLobby = false;
            }
        }

        [HttpPost]
        [Route("lobby/connect")]
        public LobbyInfo GetLobbyAddr([FromBody] LobbyInfo lobby)
        {
            var serialize = JsonConvert.SerializeObject(lobby);
            var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
            var result = client.PostAsync(dbManagerApi + "/dbmanager/lobby/connect",toSend).Result;
            var lobbyInfo = JsonConvert.DeserializeObject<LobbyInfo>(result.Content.ReadAsStringAsync().Result);
            return lobbyInfo;
        }


        // ** Méthodes ** //

        // POST request au Spotify API pour get un OAuth 2.0 Token
        // Ce token est ensuite utilisé pour faire des requêtes au Spotify API
        public Token GetNewToken(Dictionary<string, string> requestBody)
        {
            try
            {
                var response = client.PostAsync(_tokenURL, new FormUrlEncodedContent(requestBody)).Result;
                var jsonContent = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Token>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // Envoi une POST request a notre database manager
        public Token PostDbManager(string apiPath, Token dataToSend)
        {
            try
            {
                var serialize = JsonConvert.SerializeObject(dataToSend);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var result = client.PostAsync(dbManagerApi + "/dbmanager/" + apiPath, toSend).Result;
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
