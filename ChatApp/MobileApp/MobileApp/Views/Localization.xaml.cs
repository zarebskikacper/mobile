using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Localization : ContentPage
    {
        bool isGettingLocation;
        public Localization()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            isGettingLocation = true;

            while (isGettingLocation)
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));
                Znacznik.Text += $"{result.Latitude}, {result.Longitude} {Environment.NewLine}";
                await Task.Delay(1000);
            }
        }

        async void Button1_Clicked(System.Object sender, System.EventArgs e)
        {
            isGettingLocation = false;
        }

        async void Button2_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var result = await Geocoding.GetLocationsAsync(Znacznik.Text);

                if (result.Any())
                    Znacznik.Text = $"{result.FirstOrDefault()?.Latitude}, {result.FirstOrDefault().Longitude} {Environment.NewLine}";
            }
            catch (FeatureNotSupportedException notsupportedex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        async void Button3_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                double szerokosc, dlugosc;

                szerokosc = Convert.ToDouble(Znacznik.Text.Split(',')[0]);
                dlugosc = Convert.ToDouble(Znacznik.Text.Split(',')[1]);

                var results = await Geocoding.GetPlacemarksAsync(szerokosc, dlugosc);
                var result = results?.FirstOrDefault();

                if (result != null)
                {
                    var geocodeAddress =
                        $"AdminArea:         {result.AdminArea}\n"
                        + $"CountryCode:     {result.CountryCode}\n"
                        + $"CountryName:     {result.CountryName}\n"
                        + $"FeatureName:     {result.FeatureName}\n"
                        + $"Locality:        {result.Locality}\n"
                        + $"PostalCode:      {result.PostalCode}\n"
                        + $"SubAdminArea:    {result.SubAdminArea}\n"
                        + $"SubLocality:     {result.SubLocality}\n"
                        + $"SubThoroughfare: {result.SubThoroughfare}\n"
                        + $"Thoroughfare:    {result.Thoroughfare}\n";

                    LabelInfo.Text = geocodeAddress;
                }
            }
            catch (FeatureNotSupportedException notsupportedex) { }
            catch (Exception ex){}
        }

        async void Button4_Clicked(System.Object sender, System.EventArgs e)
        {
            try 
            { 
                double szerokosc, dlugosc;

                szerokosc = Convert.ToDouble(Znacznik.Text.Split(',')[0]);
                dlugosc = Convert.ToDouble(Znacznik.Text.Split(',')[1]);

                var location = new Location(szerokosc, dlugosc);

                await Map.OpenAsync(location);
            }
            catch 
            {
                LabelInfo.Text = "Lokalizacja nie może być pusta";
            }
        }
    }
}