using ClassLibrary.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using MixUp.Services;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostPage : ContentPage
    {
        private User _user;

        public HostPage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        async void OnCreateLobbyButtonClicked(object sender, EventArgs args)
        {
            // Start Server thread
            if (lobbyNameEntry.Text != null)
            {
                LobbyService lobbyService = new LobbyService();
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                RoomCodeGenerator rcg = new RoomCodeGenerator();
                string roomCode = rcg.GenerateAndInsertCode(ipAddr);
                lobbyService.SaveLobbyCode(roomCode, ipAddr.ToString());
                Server lobbyServer = new Server(_user, lobbyNameEntry.Text, roomCode);
                ThreadStart work = lobbyServer.ExecuteServer;
                Thread serverThread = new Thread(work);
                serverThread.Start();

                await Navigation.PushAsync(new LobbyPage(null, ipAddr.ToString(), serverThread, lobbyServer, _user));
            }
            else
            {
                await DisplayAlert("Invalid Name", "Please enter a valid room name", "OK");
                return;
            }
        }
    }
}