using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleThings : ContentPage
    {
        SensorSpeed speed = SensorSpeed.UI;
        bool isGettingBattery;
        public SimpleThings()
        {
            InitializeComponent();
            Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
            Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
        }

        async void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        { 
            var level = e.ChargeLevel;
            var state = e.State;
            var source = e.PowerSource;

            while (isGettingBattery)
            {
                LabelInfo.Text = ($"Odczytuję:\n Pojemność: {level * 100}%,\nStan: {state},\nŹródło: {source}");
                await Task.Delay(1000);
            }
        }

        void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            var data = e.Reading;
            LabelInfo.Text = $"Odczytuję:\n X: {data.MagneticField.X},\n Y: {data.MagneticField.Y},\n Z: {data.MagneticField.Z}";
        }


        async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Magnetometer.IsMonitoring)
                    Magnetometer.Stop();
                else
                    Magnetometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        async void Button1_Clicked(object sender, EventArgs e)
        {
            isGettingBattery = true;
        }

        async void Button2_Clicked(object sender, EventArgs e)
        {
            isGettingBattery = false;
        }

        async void Button3_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Flashlight.TurnOnAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        async void Button4_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Flashlight.TurnOffAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        async void Button5_Clicked(System.Object sender, System.EventArgs e)
        {
            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync("https://riptutorial.com/Download/xamarin-android.pdf");

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView("xamarin-android.pdf", "application/pdf", new System.IO.MemoryStream(), PDFOpenContext.InApp);
            }
        }

    }
}