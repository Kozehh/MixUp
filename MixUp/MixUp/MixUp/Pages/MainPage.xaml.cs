using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using MixUp.Pages;
using Xamarin.Forms;

namespace MixUp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnLoginPageButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginPage());
        }

    }
}
