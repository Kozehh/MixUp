using ClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
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
        public SongPage()
        {
            InitializeComponent();
            _client = new HttpClient();
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        { 
            var getResult = _client.GetAsync(mixupApi + "authenticate").Result;
            var source = new UrlWebViewSource
            {
                Url = getResult.Content.ReadAsStringAsync().Result
            };

            WebView loginView = new WebView()
            {
                Source = source,
                VerticalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 1000,
                HeightRequest = 1000
            };


            Content = new StackLayout()
            {
                Children = { loginView }
            };

            await Navigation.PushAsync(new HomePage());
            /*
            await Task.Run(async () =>
            {
                //var res = await _client.GetAsync(mixupApi + "callback");
                //var token = res.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(token);
                /*
                while (string.IsNullOrEmpty(JsonConvert.DeserializeObject<Token>(token).AccessToken))
                {
                    token = res.Content.ReadAsStringAsync().Result;
                }
                
                Console.WriteLine(getResult.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Content = new StackLayout()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Spacing = 0,
                        Children =
                        {
                            new Label()
                            {
                                Text = "Song Page",
                                HorizontalOptions = LayoutOptions.Start
                            }
                        }
                    };
                });
            });
            */

        }
    }
}