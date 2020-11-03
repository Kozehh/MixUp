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
    public partial class LobbyPage : ContentPage
    {
        Session session;
        public String ip;
        public String name;
        private Thread clientThread;
        private Thread serverThread;
        private Server server;
        public LobbyPage(String name, String ip, Thread st, Server server)
        {
            InitializeComponent();
            this.server = server;
            this.serverThread = st;
            this.ip = ip;
            this.name = name;
            // Start the Session thread
            Session lobbySession = new Session(name, this);
            this.session = lobbySession;
            System.Threading.ThreadStart clientWork = lobbySession.ExecuteClient;
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


        void OnSendButtonClicked(object sender, EventArgs args)
        {
            String messageToSend = messageEntry.Text;
            session.SendMessage(messageToSend);
        }

        public void Update(Lobby lobby)
        {
            // REFRESH PAGE
            lobbyIp.Text = lobby.ipAddress.ToString();
        }
    }
}