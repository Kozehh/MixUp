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
    public partial class HostPage : ContentPage
    {
        public HostPage()
        {
            InitializeComponent();
        }

        async void OnCreateLobbyButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
            Lobby lobby = new Lobby();
        }

        async void OnSettingsButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}