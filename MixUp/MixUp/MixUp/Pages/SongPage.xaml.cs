using ClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongPage : ContentPage
    {
        private string mixupApi = "http://10.44.88.242/mixup/";
        private HttpClient _client;
        static WebView loginView;
        public SongPage()
        {
            InitializeComponent();
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

                loginView = new WebView()
                {
                    Source = source,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    WidthRequest = 1000,
                    HeightRequest = 1000
                };
                loginView.Navigated += view_Navigated;

                Content = new StackLayout()
                {
                    Children = {loginView}
                };
                

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                await Navigation.PushAsync(new MainPage());
            }
        }

        // Get the auth code from the Spotify redirect
        // And get the token afterward
        public async void view_Navigated(object sender, EventArgs e)
        {
            var authCode = await loginView.EvaluateJavaScriptAsync("document.body.innerText");

            var serialize = JsonConvert.SerializeObject(authCode);
            var toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
            var result = _client.PostAsync(mixupApi + "requestToken", toSend).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            var token = JsonConvert.DeserializeObject<Token>(json);

            serialize = JsonConvert.SerializeObject(token);
            toSend = new StringContent(serialize, Encoding.UTF8, "application/json");
            result = _client.PostAsync(mixupApi + "user", toSend).Result;
            json = result.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(json);
            //var res = _client.GetAsync()
            await Navigation.PushAsync(new HomePage(token));
        }
    }
}