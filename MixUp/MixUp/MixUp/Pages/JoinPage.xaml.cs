using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinPage : ContentPage
    {
        public JoinPage()
        {
            InitializeComponent();
        }

        async void OnJoinClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LobbyPage(null, ipEntry.Text, null, null));
        }
    }
}