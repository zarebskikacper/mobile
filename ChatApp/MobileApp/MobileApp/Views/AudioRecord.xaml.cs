using Plugin.AudioRecorder;
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
    public partial class AudioRecord : ContentPage
    {
        private readonly AudioRecorderService recorder;
        private readonly AudioPlayer player;
        public AudioRecord()
        {
            InitializeComponent();

            recorder = new AudioRecorderService();
            player = new AudioPlayer();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
                return;

            if (recorder.IsRecording)
            {
                LabelInfo.Text = "Nagrywanie zatrzymane";
                await recorder.StopRecording();
            }
            else
            {
                LabelInfo.Text = "Nagrywanie rozpoczęte";
                await recorder.StartRecording();
            }
        }

        void Button1_Clicked(System.Object sender, System.EventArgs e)
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
    }
}