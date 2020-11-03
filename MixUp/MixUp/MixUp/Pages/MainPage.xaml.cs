using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using Xamarin.Forms;

namespace MixUp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.HomePage(new Token()));
        }

        async void OnPlayerShortcutButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.MediaPlayer());
        }

        async void OnSongPageButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.SongPage());
        }

    }
}
