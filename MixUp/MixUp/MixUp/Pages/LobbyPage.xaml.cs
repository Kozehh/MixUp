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
        public LobbyPage(String name)
        {
            InitializeComponent();
            this.name = name;
            // Start the Session thread
            Session lobbySession = new Session(name, this);
            this.session = lobbySession;
            System.Threading.ThreadStart clientWork = lobbySession.ExecuteClient;
            clientThread = new Thread(clientWork);
            clientThread.Start();
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