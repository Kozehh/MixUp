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
        public LobbyPage(String name, String ip)
        {
            InitializeComponent();

            this.ip = ip;

            // Start the Session thread
            Session lobbySession = new Session(this);
            this.session = lobbySession;


            System.Threading.ThreadStart clientWork = lobbySession.ExecuteClient;
            Thread clientThread = new Thread(clientWork);
            clientThread.Start();

        }

        void OnSendButtonClicked(object sender, EventArgs args)
        {
            String messageToSend = messageEntry.Text;
            session.connectionManager.SendMessage(messageToSend);
        }

        public void Update()
        {
            // TO DO : REFRESH PAGE
        }
    }
}