﻿using Newtonsoft.Json;
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


            await Task.Run(async () =>
            {
                var isFinished = await _client.GetAsync(mixupApi + "auth-finished");
                while (String.Equals(isFinished.Content.ReadAsStringAsync().Result, "false"))
                {
                    isFinished = await _client.GetAsync(mixupApi + "auth-finished");
                }

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



        }
    }
}