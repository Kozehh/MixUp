﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Models;
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
            Server lobbyServer = new Server(null);
            ThreadStart work = lobbyServer.ExecuteServer;
            Thread serverThread = new Thread(work);
            serverThread.Start();

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            
            await Navigation.PushAsync(new LobbyPage(null, ipAddr.ToString(), serverThread, lobbyServer, _user));
        }

        async void OnSettingsButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }


    }
}