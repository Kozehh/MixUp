using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinPage : ContentPage
    {
        private User user;
        public JoinPage(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        async void OnJoinClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LobbyPage(null, ipEntry.Text, null, null, user));
        }
    }
}