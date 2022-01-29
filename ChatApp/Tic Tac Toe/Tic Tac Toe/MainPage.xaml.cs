using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tic_Tac_Toe
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        Library library = new Library();

        private void New_Clicked(object sender, EventArgs e)
        {
            library.New(this, Display);
        }
    }
}
