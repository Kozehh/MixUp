using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Server lobbyServer = new Server(null);
            System.Threading.ThreadStart work = lobbyServer.ExecuteServer;
            Thread serverThread = new Thread(work);
            serverThread.Start();

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            String ip = ipAddr.ToString();
            
            await Navigation.PushAsync(new Pages.LobbyPage(null, ip, serverThread, lobbyServer));
        }


    }
}