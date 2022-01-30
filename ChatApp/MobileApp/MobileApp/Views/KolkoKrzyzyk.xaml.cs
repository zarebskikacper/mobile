using MobileApp.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KolkoKrzyzyk : ContentPage
    {
       
        public KolkoKrzyzyk()
        {
            InitializeComponent();
        }

        tttGame game = new tttGame();
        private void Button_Clicked(object sender, EventArgs e)
        {
            game.New(this, Display);
        }
    }
}