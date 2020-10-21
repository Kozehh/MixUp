using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongPage : ContentPage
    {
        private string _mixUpApi = "http://10.44.88.242:9000/MixUpApi/Authenticate";
        private HttpClient _client;
        public SongPage()
        {
            InitializeComponent();
            _client = new HttpClient();
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        { 
            var getResult = _client.GetAsync(_mixUpApi).Result;
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

            this.Content = new StackLayout()
            {
                Children = { loginView }
            };


            //var res = _client.GetAsync("http://10.0.2.2:5000/MixUpApi/db").Result;

        }
    }
}