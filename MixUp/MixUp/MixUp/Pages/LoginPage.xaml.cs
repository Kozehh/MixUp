using Android.Webkit;
using ClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WebView = Xamarin.Forms.WebView;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        private const string mixupApi = @"http://192.168.0.162:9000/mixup/";
        private const string callback = "http://192.168.0.162:9000/mixup/callback";
        private HttpClient _client;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _web;
        private bool _load;
        private WebView _loginView;
        private StackLayout stack;
        private bool finishedLogin;

        // Display the loading overlay when property is set to true
        public bool Load
        {
            get { return _load; }
            set { _load = value; OnPropertyChanged();}
        }

        // Stop displaying the web view when property is set to false
        // We do that because the token received is shown on the view
        public bool Web
        {
            get { return _web; }
            set { _web = value; OnPropertyChanged(); }
        }

        // Update the property on the UI
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // LoginPage constructor
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
            _load = false;
            _web = true;
            finishedLogin = false;
            stack = this.FindByName<StackLayout>("log");
            _loginView = this.FindByName<WebView>("loginview");
            // Handle Navigated and Navigating events of our webview
            _loginView.Navigated += View_Navigated;
            _loginView.Navigating += (sen, e) =>
            {
                // If we are naviagting to the callback URL, the login is finished
                if (e.Url.Contains(callback))
                {
                    finishedLogin = true;
                }
            };
            // Create an http client and set a 4 sec timeout
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(4)
            };
            
            ClearCookies();
        }

        // Login to spotify
        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            try
            {
                // Call our API to handle the authentification request
                var getResult = _client.GetAsync(mixupApi + "authenticate").Result;
                var re = await getResult.Content.ReadAsStringAsync();
                // Receive the URL to login to spotify and update our webview source
                var source = new UrlWebViewSource
                {
                    Url = re
                };
                _loginView.Source = source;
                stack.IsVisible = true;
                Content = stack;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Navigation.PushAsync(new LoginPage());
            }
        }

        // Get the auth code from the Spotify redirect
        // And get the token afterward
        public async void View_Navigated(object sender, EventArgs e)
        {
            // If the login process is finished, we redirect the user to the HomePage
            if (finishedLogin)
            {
                Web = false;
                Load = true;
                var token = await GetToken();
                var user = GetUser(token);
                user.Token = token;
                Load = false;
                await Navigation.PushAsync(new HomePage(user));
            }
        }

        // Getting the user code
        private async Task<Token> GetToken()
        {
                var authCode = await _loginView.EvaluateJavaScriptAsync("document.body.innerText");
                var serialize = JsonConvert.SerializeObject(authCode);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var result = _client.PostAsync(mixupApi + "requestToken", toSend).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Token>(json);
        }

        // Get Spotify's user info
        private User GetUser(Token token)
        {
            var serialize = JsonConvert.SerializeObject(token);
            var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
            var result = _client.PostAsync(mixupApi + "user", toSend).Result;
            return JsonConvert.DeserializeObject<User>(result.Content.ReadAsStringAsync().Result);
        }

        // Clear the webview cookies
        public void ClearCookies()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}