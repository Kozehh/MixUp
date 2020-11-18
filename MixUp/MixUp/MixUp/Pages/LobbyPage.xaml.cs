﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Models;
using MixUp.Services;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LobbyPage : ContentPage
    {
        Session session;
        public string ip;
        public string name;
        private List<Song> Queue;
        private Thread clientThread;
        private Thread serverThread;
        private Server server;
        private User HostUser;
        public const string songToAdd = "/cAddSong:";

        public LobbyPage(string name, string ip, Thread st, Server server, User user)
        {
            InitializeComponent();
            this.server = server;
            this.serverThread = st;
            HostUser = user;
            this.ip = ip;
            this.name = name;
            Queue = new List<Song>();
            // Start the Session thread
            Session lobbySession = new Session(name, this);
            this.session = lobbySession;
            ThreadStart clientWork = lobbySession.ExecuteClient;
            clientThread = new Thread(clientWork);
            clientThread.Start();
        }
        
        async void OnDisconnectButtonClicked(object sender, EventArgs args)
        {
            if(serverThread != null)
            {
                foreach(Thread t in server.serverThreads)
                {
                    t.Abort();
                }
                serverThread.Abort();
            }
            clientThread.Abort();
            await Navigation.PopAsync();
        }

        public void Update(Lobby lobby)
        {
            if (lobby.songList.Count > 0)
            {
                foreach(Song s in lobby.songList)
                {
                    Label label = new Label { Text = s.Name, CharacterSpacing = 10 };
                    //songList.Children.Add(label);
                }
            }
            lobbyIp.Text = lobby.ipAddress.ToString();
        }

        void OnSendButtonClicked(object sender, EventArgs args)
        {
            //String messageToSend = messageEntry.Text;
            //session.SendMessage(messageToSend);
        }

        async void OnAddSongClicked(object sender, EventArgs args)
        {
            session.SendMessage(songToAdd + "2gHA5uelC4cmT0Rn91rTm1");
        }

        async void OnMusicPageClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MusicPage(HostUser));
        }
    }
}