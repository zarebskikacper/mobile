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
    public partial class ContactViews : ContentPage
    {
        public ContactViews()
        {
            InitializeComponent();
        }
        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var contact = await Xamarin.Essentials.Contacts.PickContactAsync();
                if (contact == null)
                    return;

                var info = new StringBuilder();
                info.AppendLine("ID: " + contact.Id);
                info.AppendLine("Imię: " + contact.GivenName);
                info.AppendLine("Drugie Imię: " + contact.MiddleName);
                info.AppendLine("Nazwisko: " + contact.FamilyName);
                info.AppendLine("Nazwa kontaktu: " + contact.DisplayName);
                info.AppendLine("Numer telefonu: " + contact.Phones.FirstOrDefault()?.PhoneNumber ?? String.Empty);
                info.AppendLine("Adres E-mail: " + contact.Emails.FirstOrDefault()?.EmailAddress ?? String.Empty);

                Kontakty.Text = contact.Phones?.FirstOrDefault()?.PhoneNumber ?? String.Empty;
                AddressEmail.Text = contact.Emails.FirstOrDefault()?.EmailAddress ?? String.Empty;
                LabelInfo.Text = info.ToString();
            }
            catch (Exception ex)
            { }
        }

        async void Button1_Clicked(object sender, EventArgs e)
        {
            var recipent = Kontakty.Text;
            var message = Message.Text;

            await SendSms(message, recipent);
        }

        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, recipient);
                await Sms.ComposeAsync(message);
                Kontakty.Text = "";
                Message.Text = "";
                AddressEmail.Text = "";
                Subject.Text = "";
            }

            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                LabelInfo.Text = "Nie udało się wysłać wiadomości";
            }
        }

        async void Button2_Clicked(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open(Kontakty.Text);
            }
            catch (Exception)
            {
                DisplayAlert("Nie można wykonać połączenia", "Proszę wprowadzić numer", "OK");
            }
        }

        async void Button3_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<string> result = AddressEmail.Text.Split(',').ToList();

                var subject = Subject.Text;
                var message = Message.Text;
                var emailto = result;

                var email = new EmailMessage
                {
                    Subject = subject,
                    Body = message,
                    To = emailto,
                };
                await Email.ComposeAsync(email);
                Kontakty.Text = "";
                Message.Text = "";
                AddressEmail.Text = "";
                Subject.Text = "";
            }
            catch (Exception ex)
            {

            }
        }
    }
}