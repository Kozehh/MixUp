using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            await Navigation.PushAsync(new Pages.HomePage());
        }

        async void OnLobbyShortcutButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.HomePage());
        }

        async void OnSongPageButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.SongPage());
        }

    }
}
