using ClassLibrary.Models;
using System;
using MixUp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinPage : ContentPage
    {
        private User user;
        public JoinPage(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        public async void OnJoinClicked(object sender, EventArgs args)
        {
            LobbyService service = new LobbyService();
            if (ipEntry.Text != null && ipEntry.Text.Length == 6)
            {
                RoomCodeGenerator rcg = new RoomCodeGenerator();
                LobbyInfo lobby = rcg.GetRoomAddress(ipEntry.Text);

                if (lobby != null)
                {
                    await Navigation.PushAsync(new LobbyPage(null, lobby.LobbyAddr, null, null, user));
                }

                else
                {
                    await DisplayAlert("Invalid Code", "Please try again", "OK");
                }

            }
            else
            {
                await DisplayAlert("Invalid Code", "Please enter a 6 character room code", "OK");
            }

        }
    }
}