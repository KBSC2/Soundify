using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.MailTemplates;
using View.DataContexts;

namespace View
{
    /// <summary>
    /// Interaction logic for RequestArtist.xaml
    /// </summary>
    public partial class RequestArtist : Window
    {
        public RequestArtist()
        {
            InitializeComponent();
        }

        private void Confirm_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirm_Button_Click(sender, new RoutedEventArgs());
            }
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var artistName = this.ArtistName.Text;
            var artistReason = this.ArtistReason.Text;

            var emailController = new EmailController();
            var email =  new MailArtistVerification(new MailAddress("info.soundify@gmail.com"), artistName, artistReason);
            emailController.SendEmail(email, "info.soundify@gmail.com");

            var request = new Request()
            {
                ArtistName = artistName, 
                ArtistReason = artistReason, 
                UserID = UserController.CurrentUser.ID,
                RequestType = RequestType.Artist,
                SongID = null
            };

            var userController = UserController.Create(new DatabaseContext());
            userController.GetItem(UserController.CurrentUser.ID).ContinueWith(res =>
            {
                var user = res.Result;
                user.RequestedArtist = true;
                userController.UpdateItem(user);
            });

            var createRequest = RequestController.Create(new DatabaseContext());
            createRequest.CreateItem(request);

            this.Close();

            SettingsDataContext.Instance.OnPropertyChanged("");
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
