using ClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        private string mixupApi = @"http://10.44.88.242/mixup/";
        private HttpClient _client;
        private User _user;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _web;
        private bool _load;
        private WebView _loginView;
        private StackLayout stack;
        private bool finishedLogin;

        public bool Load
        {
            get { return _load; }
            set { _load = value; OnPropertyChanged();}
        }

        public bool Web
        {
            get { return _web; }
            set { _web = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
            _load = false;
            _web = true;
            finishedLogin = false;
            stack = this.FindByName<StackLayout>("log");
            _loginView = this.FindByName<WebView>("loginview");
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(4)
            };
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            try
            {
                var getResult = _client.GetAsync(mixupApi + "authenticate").Result;
                var source = new UrlWebViewSource
                {
                    Url = getResult.Content.ReadAsStringAsync().Result
                };
                _loginView.Source = source;
                _loginView.Navigated += View_Navigated;
                _loginView.Navigating += (sen, e) =>
                {
                    if (e.Url.Contains(mixupApi))
                    {
                        finishedLogin = true;
                    }
                };
                stack.IsVisible = true;
                Content = stack;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Navigation.PushAsync(new MainPage());
            }
        }

        // Get the auth code from the Spotify redirect
        // And get the token afterward
        public async void View_Navigated(object sender, EventArgs e)
        {
            if (finishedLogin)
            {
                Web = false;
                Load = true;
                await GetCode();
                // TODO: Save user info in DB
                Load = false;
                await Navigation.PushAsync(new HomePage(_user));
            }
        }

        public async Task GetCode()
        {
                HttpClient c = new HttpClient();
                var authCode = await _loginView.EvaluateJavaScriptAsync("document.body.innerText");
                var serialize = JsonConvert.SerializeObject(authCode);
                var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
                var result = c.PostAsync(mixupApi + "requestToken", toSend).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<Token>(json);

                // Get Spotify's user info
                HttpClient client = new HttpClient();
                var s = JsonConvert.SerializeObject(token);
                var send = new StringContent(s, Encoding.UTF8, "application/json");
                var r = client.PostAsync(mixupApi + "user", send).Result;
                _user = JsonConvert.DeserializeObject<User>(r.Content.ReadAsStringAsync().Result);
                _user.Token = token;
        }
    }
}