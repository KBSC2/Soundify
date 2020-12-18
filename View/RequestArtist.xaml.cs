using System.Net.Mail;
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

            // Makes screen in universal font
            Style = (Style)FindResource(typeof(Window));
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
            var email = new MailArtistVerification(new MailAddress("info.soundify@gmail.com"), artistName, artistReason);

            var controller = RequestController.Create(DatabaseContext.Instance);
            var result = controller.RequestArtist(artistName, artistReason);
            switch (result)
            {
                case RequestArtistResults.Success:
                    {
                        emailController.SendEmail(email, "info.soundify@gmail.com");

                        var request = new Request()
                        {
                            ArtistName = artistName,
                            ArtistReason = artistReason,
                            UserID = UserController.CurrentUser.ID,
                            RequestType = RequestType.Artist,
                            SongID = null
                        };

                        var userController = UserController.Create(DatabaseContext.Instance);
                        var user = userController.GetItem(UserController.CurrentUser.ID);
                        user.RequestedArtist = true;
                        userController.UpdateItem(user);

                        var createRequest = RequestController.Create(DatabaseContext.Instance);
                        createRequest.CreateItem(request);
                        this.Close();

                        MessageBox.Show("Request Artist is confirmed!");
                        SettingsDataContext.Instance.OnPropertyChanged("");
                    }

                        this.ArtistName.Text = "";
                        this.ArtistReason.Text = "";
                        break;
                    
                case RequestArtistResults.ArtistNameNotFound:
                    {
                        this.Error.Content ="Your artist name is empty, please go back and fill in your artist name before proceeding.";
                        break;
                    }

                case RequestArtistResults.ReasonNotFound:
                    {
                        this.Error.Content = "Please fill in your reason as to why you want to become an artist before proceeding.";
                        break;
                    }

                case RequestArtistResults.NameAndReasonNotFound:
                {
                        this.Error.Content = "Please fill in your Artist name and reason to become an artist before proceeding.";
                        break;
                }
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
