using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.AudioRecorder;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System.Net.Http;
using System.IO;
using MobileApp.Views;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        private readonly AudioRecorderService recorder;
        private readonly AudioPlayer player;

        public MainPage()
        {
            InitializeComponent();

            recorder = new AudioRecorderService();
            player = new AudioPlayer();
        }      

        async void Photo_Collection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoPicker());
        }

        async void Localization_Collection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Localization());
        }

        async void TicTacToe(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new KolkoKrzyzyk());
        }

        async void Contacts_Collection(System.Object sender, System.EventArgs e)
        {
            var availability = await CrossFingerprint.Current.IsAvailableAsync();

            if (!availability)
            {
                await DisplayAlert("Uwaga!", "Funkcje biometryczne niedostępne", "OK");
                return;
            }

            var authResult = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration("Uwaga!", "Potrzeba Twoich danych biometrycznych"));
            if (authResult.Authenticated)
            {
                await Navigation.PushAsync(new ContactViews());
            }
        }

        async void Things_Collection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SimpleThings());
        }
        
        async void Record_Collection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AudioRecord());
        }

        async void Azure_Collection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AzureStorage());
        }
    }
}