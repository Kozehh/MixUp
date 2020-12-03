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

        async void OnJoinClicked(object sender, EventArgs args)
        {
            LobbyService service = new LobbyService();
            if (ipEntry.Text != null && ipEntry.Text.Length == 6)
            {
                RoomCodeGenerator rcg = new RoomCodeGenerator();
                string ipAddress = rcg.GetRoomAddress(ipEntry.Text);
                var lobby = service.FindLobbyWithCode(ipEntry.Text);

                if (lobby.LobbyAddr != "")
                {
                    await Navigation.PushAsync(new LobbyPage(null, lobby.LobbyAddr, null, null, user));
                }

                else
                {
                    await DisplayAlert("Invalid Code", "Please enter a 6 character room code", "OK");
                }

            }
            else
            {
                await DisplayAlert("Invalid Code", "Please enter a 6 character room code", "OK");
                return;
            }

        }
    }
}