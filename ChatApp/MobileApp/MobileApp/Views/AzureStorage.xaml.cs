using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Azure.Storage.Blobs;
using Plugin.Media;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using System.IO;
using Azure.Storage.Blobs.Models;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AzureStorage : ContentPage
    {
        MediaFile file;
        static string _storageConnection = "DefaultEndpointsProtocol=https;AccountName=mobileappfunc;AccountKey=AJHdxNm6IRXAS0W4i8Rhqk1FYX0T9Lu8HGmy75MjlibO4OWOyHUd6U2LFeualRdUvTooqwEAxp+2mx/mBMWa1A==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_storageConnection);
        static CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        static CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("images");
        
        public AzureStorage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                PlaceImage.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void Button1_Clicked(object sender, EventArgs e)
        {
            try 
            {
                string filePath = file.Path;
                string fileName = Path.GetFileName(filePath);
                await cloudBlobContainer.CreateIfNotExistsAsync();

                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                await UploadImage(blockBlob, filePath);
                LabelInfo.Text = "Zdjęcie zostało wysłane";
            }
            catch { }
        }

        private async void Button2_Clicked(object sender, EventArgs e)
        {
            string filePath = file.Path;
            string fileName = Path.GetFileName(filePath);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference("minion-clipart-2018-34_10.jpeg");
            await DownloadImage(blockBlob, filePath);
        }

        private async void Button3_Clicked(object sender, EventArgs e)
        {

            var blockBlob = cloudBlobContainer.GetBlockBlobReference("minion-clipart-2018-34_10.jpeg");
            await DeleteImage(blockBlob);
        }

        private static async Task UploadImage(CloudBlockBlob blob, string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
        }

        private static async Task DownloadImage(CloudBlockBlob blob, string filePath)
        {
            if (blob.ExistsAsync().Result)
            {
                await blob.DownloadToFileAsync(filePath, FileMode.CreateNew);
            }
        }

        private static async Task DeleteImage(CloudBlockBlob blob)
        {
            if (blob.ExistsAsync().Result)
            {
                await blob.DeleteAsync();
            }
        }
    }
}