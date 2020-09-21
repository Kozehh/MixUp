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
            System.Threading.ThreadStart work = LobbyServer.ExecuteServer;
            Thread serverThread = new Thread(work);
            serverThread.Start();
            //Lobby lobby = new Lobby();
            await Navigation.PushAsync(new Pages.LobbyPage());
        }

        async void OnSettingsButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }


    }
}