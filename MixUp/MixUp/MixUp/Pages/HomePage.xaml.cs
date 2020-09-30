using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MixUp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        async void OnHostButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.HostPage());
        }

        async void OnJoinButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Pages.LobbyPage(nameEntry.Text, ipEntry.Text));
        }

    }
}