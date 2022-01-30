using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPicker : ContentPage
    {
        public PhotoPicker()
        {
            InitializeComponent();
            //string localPath = Path.Combine(FileSystem.CacheDirectory, localFileName);
        }

        async void Button_Clicked(object sender, EventArgs E)
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
    }
}