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

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        bool isGettingLocation;
        private readonly AudioRecorderService recorder;
        private readonly AudioPlayer player;
        public MainPage()
        {
            InitializeComponent();

            recorder = new AudioRecorderService();
            player = new AudioPlayer();

            Bateria.Text = (Battery.ChargeLevel * 100).ToString() + "%";
            Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;

            //Task.Run(Animacja);
        }

        /*private async void Animacja()
        {
            Action<double> forward = input => Tlo.AnchorY = input;
            Action<double> backward = input => Tlo.AnchorY = input;

            while(true)
            {
                Tlo.Animate(name: "forward", callback: forward, start: 0, end: 2, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
                Tlo.Animate(name: "backward", callback: backward, start: 0, end: 2, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
            }
        }*/

        private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        {
            Bateria.Text = (e.ChargeLevel * 100).ToString() + "%";
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Proszę wybrać zdjęcie"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                Zdjęcie.Source = ImageSource.FromStream(() => stream);
            }
        }

        async void Button1_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                Zdjęcie.Source = ImageSource.FromStream(() => stream);
            }
        }

        async void Button2_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var result = await Contacts.PickContactAsync();

                if (result != null)
                    Kontakty.Text = $"{result.ToString()} {result.Phones.First()}";
            }
            catch (Exception ex)
            {

            }
        }

        async void Button3_Clicked(System.Object sender, System.EventArgs e)
        {
            isGettingLocation = true;

            while (isGettingLocation)
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));
                Lokalizacja.Text += $"Szerokość: {result.Latitude}, Długość: {result.Longitude} {Environment.NewLine}";
                await Task.Delay(1000);
            }
        }

        async void Button4_Clicked(System.Object sender, System.EventArgs e)
        {
            isGettingLocation = false;
        }

        async void Button5_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var result = await Geocoding.GetLocationsAsync(Znacznik.Text);

                if (result.Any())
                    Lokalizacja.Text = $"Szerokość: {result.FirstOrDefault()?.Latitude}, Długość {result.FirstOrDefault().Longitude} {Environment.NewLine}";
            }
            catch (FeatureNotSupportedException notsupportedex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        async void Button6_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                double szerokosc, dlugosc;

                szerokosc = Convert.ToDouble(Znacznik.Text.Split(',')[0]);
                dlugosc = Convert.ToDouble(Znacznik.Text.Split(',')[1]);

                var result = await Geocoding.GetPlacemarksAsync(szerokosc, dlugosc);

                if (result.Any())
                    Lokalizacja.Text = result.FirstOrDefault().FeatureName;
            }
            catch (FeatureNotSupportedException notsupportedex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        async void Button7_Clicked(System.Object sender, System.EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
                return;

            if (recorder.IsRecording)
            {
                Lokalizacja.Text = "Nagrywanie zatrzymane";
                await recorder.StopRecording();
            }
            else
            {
                Lokalizacja.Text = "Nagrywanie rozpoczęte";
                await recorder.StartRecording();
            }
        }

        void Button8_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var filePath = recorder.GetAudioFilePath();
                if (filePath != null)
                {
                    player.Play(filePath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void Button9_Clicked(System.Object sender, System.EventArgs e)
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
                await DisplayAlert("Ok","Twoje zabezpieczone dane", "OK");
            }
        }

        async void Button10_Clicked(System.Object sender, System.EventArgs e)
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync("https://riptutorial.com/Download/xamarin-android.pdf");

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView("myFile.pdf", "application/pdf", new System.IO.MemoryStream(), PDFOpenContext.InApp);
            }
        }
    }
}