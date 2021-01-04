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
        }

        private void UploadSongButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog { DefaultExt = ".mp3", Filter = "MP3 Files (.mp3)|*.mp3" };

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
            var dataGrid = (Grid)((Button)sender).Parent;
            var songName = ((TextBox)dataGrid.FindName("Title"))?.Text;

            if (songName == "")
            {
                MessageBox.Show("Title must be set", "Error", MessageBoxButton.OK);
                return;
            }

            var imageSource = ((BitmapImage)((Image)dataGrid.FindName("Image"))?.Source)?.UriSource;

            if (imageSource != null) FileTransfer.Create(DatabaseContext.Instance).UploadFile(imageSource?.LocalPath, "images/" +
                imageSource.LocalPath.Split("\\").Last());

            var artist = ArtistController.Create(DatabaseContext.Instance).GetArtistFromUser(UserController.CurrentUser);
            if (artist == null) return;

            var song = new Song()
            {
                Name = songName,
                ArtistID = artist.ID,
                Description = dataGrid.FindName("Description") != null
                    ? ((TextBox) dataGrid.FindName("Description"))?.Text
                    : null,
                Duration = TimeSpan.Parse(((Label) dataGrid.FindName("Duration"))?.Content.ToString() ?? string.Empty)
                    .TotalSeconds,
                Path = ArtistDataContext.Instance.SelectedSong.Name,
                ProducedBy = dataGrid.FindName("Producer") != null
                    ? ((TextBox) dataGrid.FindName("Producer"))?.Text
                    : null,
                WrittenBy = dataGrid.FindName("Writer") != null ? ((TextBox) dataGrid.FindName("Writer"))?.Text : null,
                Status = SongStatus.AwaitingApproval
            };

            var sController = SongController.Create(DatabaseContext.Instance);

            sController.UploadSong(song, ArtistDataContext.Instance.SelectedSong.Name);

            song.PathToImage = FileTransfer.Create(DatabaseContext.Instance).UploadFile(imageSource?.LocalPath, "images/song_" +
                string.Format("{0:}", song.ID.ToString().PadLeft(4, '0')) + "." + imageSource.LocalPath.Split("\\").Last().Split(".").Last());
            sController.UpdateItem(song);


            var request = new Request()
            {
                ArtistName = artist.ArtistName,
                UserID = UserController.CurrentUser.ID,
                RequestType = RequestType.Song,
                SongID = song.ID
            };

            var requestController = RequestController.Create(DatabaseContext.Instance);
            requestController.CreateItem(request);

            ArtistDataContext.Instance.SelectedSong = null;
            ArtistDataContext.Instance.IsSongSelected = false;
            ArtistDataContext.Instance.StatusMessage = "Awaiting Approval";
            ArtistDataContext.Instance.ArtistHasSongPending = true;

            ArtistDataContext.Instance.OnPropertyChanged("");
            SongListDataContext.Instance.OnPropertyChanged("");

            var emailController = new EmailController();
            var email = new MailSongApprovalTemplate(new MailAddress("info.soundify@gmail.com"), artist.ArtistName, songName);
            emailController.SendEmail(email, "info.soundify@gmail.com");
        }
    }
}
