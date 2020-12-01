using ClassLibrary.Models;
using System;
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
            if (ipEntry.Text != null && ipEntry.Text.Length == 6)
            {
                RoomCodeGenerator rcg = new RoomCodeGenerator();
                String ipAddress = rcg.GetRoomAddress(ipEntry.Text);

                if (ipAddress != "")
                {
                    await Navigation.PushAsync(new LobbyPage(null, ipAddress, null, null, user));
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