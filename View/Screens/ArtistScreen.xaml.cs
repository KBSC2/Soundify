using System;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Controller;
using Controller.DbControllers;
using Microsoft.Win32;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.MailTemplates;
using View.DataContexts;

namespace View.Screens
{
    partial class ArtistScreen : ResourceDictionary
    {
        public ArtistScreen()
        {
            InitializeComponent();
            SongListDataContext.Instance.ScreenName = ScreenNames.ArtistScreen;
        }

        private void UploadSongButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog {DefaultExt = ".mp3", Filter = "MP3 Files (.mp3)|*.mp3"};

            if (fileDialog.ShowDialog() != true) return;
            
            var file = TagLib.File.Create(fileDialog.FileName);

            ArtistDataContext.Instance.SelectedSong = file;
            ArtistDataContext.Instance.IsSongSelected = true;
            ArtistDataContext.Instance.StatusMessage = $"Uploading {fileDialog.SafeFileName}";

            ArtistDataContext.Instance.OnPropertyChanged("");
        }

        private void UploadSongImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog { Filter = "Files|*.jpg;*.jpeg;*.png;" };

            if (fileDialog.ShowDialog() != true) return;

            ArtistDataContext.Instance.SongImage = new BitmapImage(new Uri(fileDialog.FileName));

            ArtistDataContext.Instance.OnPropertyChanged("");
        }

        private void ConfirmUpload_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = (Grid) ((Button) sender).CommandParameter;
            var titleTextBox = (TextBox) ((Button) sender).Tag;

            if (titleTextBox.Text == "")
            {
                MessageBox.Show("Title must be set", "Error", MessageBoxButton.OK);
                return;
            }

            var imageSource = ((BitmapImage)((Image)dataGrid.FindName("Image"))?.Source)?.UriSource;

            if (imageSource != null) FileTransfer.Create(new DatabaseContext()).UploadFile(imageSource?.LocalPath, "images/" + 
                imageSource?.LocalPath.Split("\\").Last());

            var artistName = ((Label)dataGrid.FindName("Artist"))?.Content.ToString();
            var songName = ((TextBox)dataGrid.FindName("Title"))?.Text;

            SongController.Create(new DatabaseContext()).UploadSong(new Song {
                Name = songName, 
                Artist = artistName, 
                Description = ((TextBox)dataGrid.FindName("Description"))?.Text, 
                Duration = TimeSpan.Parse(((Label)dataGrid.FindName("Duration"))?.Content.ToString() ?? string.Empty).TotalSeconds, 
                Path = ArtistDataContext.Instance.SelectedSong.Name, 
                PathToImage = imageSource != null ? "images/" + imageSource.LocalPath.Split("\\").Last() : "", 
                ProducedBy = ((TextBox)dataGrid.FindName("Producer"))?.Text, 
                WrittenBy = ((TextBox)dataGrid.FindName("Writer"))?.Text,
                Status = SongStatus.AwaitingApproval
            }, ArtistDataContext.Instance.SelectedSong.Name);

            ArtistDataContext.Instance.SelectedSong = null;
            ArtistDataContext.Instance.IsSongSelected = false;
            ArtistDataContext.Instance.StatusMessage = "Awaiting Approval";
            ArtistDataContext.Instance.ArtistHasSongPending = true;

            ArtistDataContext.Instance.OnPropertyChanged("");

            var emailController = new EmailController();
            var email = new MailSongApprovalTemplate(new MailAddress("info.soundify@gmail.com"), artistName, songName);
            emailController.SendEmail(email, "info.soundify@gmail.com");
        }
    }
}
