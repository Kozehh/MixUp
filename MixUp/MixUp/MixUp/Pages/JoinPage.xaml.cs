using ClassLibrary.Models;
using System;
using System.Net;
using MixUp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinPage : ContentPage
    {
        private User user;
        private readonly IPAddress _androidEmulatorIp = IPAddress.Parse("192.168.232.2");
        public JoinPage(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        public async void OnJoinClicked(object sender, EventArgs args)
        {
            if (ipEntry.Text != null && ipEntry.Text.Length == 6)
            {
                RoomCodeGenerator rcg = new RoomCodeGenerator();
                LobbyInfo lobby = rcg.GetRoomAddress(ipEntry.Text);
                if (lobby != null)
                {
                    // Check si l'IP de la machine est l'adresse d'un emulator
                    // Si c'est le cas, on utilise l'addresse 10.0.2.2 pour communiquer au loopback address de la machine host de l'emulator
                    if (Equals(lobby.LobbyAddr, _androidEmulatorIp.ToString()))
                    {
                        lobby.LobbyAddr = IPAddress.Parse("10.0.2.2").ToString();
                    }
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