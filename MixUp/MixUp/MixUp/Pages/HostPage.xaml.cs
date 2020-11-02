using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostPage : ContentPage
    {
        public HostPage()
        {
            InitializeComponent();
        }

        async void OnCreateLobbyButtonClicked(object sender, EventArgs args)
        {
            // Start Server thread
            Server lobbyServer = new Server(nameEntry.Text);
            System.Threading.ThreadStart work = lobbyServer.ExecuteServer;
            Thread serverThread = new Thread(work);
            serverThread.Start();

            await Navigation.PushAsync(new Pages.LobbyPage(nameEntry.Text));
        }

        async void OnSettingsButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }


    }
}