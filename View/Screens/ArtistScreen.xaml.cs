using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Controller;
using Controller.DbControllers;
using Microsoft.Win32;
using Model.Database.Contexts;
using Model.DbModels;
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

            var file = TagLib.File.Create(fileDialog.FileName);

            ArtistDataContext.Instance.SongImage = new BitmapImage(new Uri(fileDialog.FileName));

            ArtistDataContext.Instance.OnPropertyChanged("");
        }

        private void ConfrimUpload_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = (Grid) ((Button) sender).CommandParameter;
            var titleTextBox = (TextBox) ((Button) sender).Tag;

            if (titleTextBox.Text == "")
            {
                MessageBox.Show("Title must be set", "Error", MessageBoxButton.OK);
                return;
            }

            var imageSouce = ((BitmapImage)((Image)dataGrid.FindName("Image"))?.Source)?.UriSource;

            FileTransfer.UploadFile(imageSouce?.LocalPath, "images/" + 
                imageSouce?.LocalPath.Split("\\").Last());

            new SongController(new DatabaseContext()).UploadSong(new Song {
                Name = ((TextBox)dataGrid.FindName("Title"))?.Text, 
                Artist = ((Label)dataGrid.FindName("Artist"))?.Content.ToString(), 
                Description = ((TextBox)dataGrid.FindName("Description"))?.Text, 
                Duration = TimeSpan.Parse(((Label)dataGrid.FindName("Duration"))?.Content.ToString() ?? string.Empty).TotalSeconds, 
                Path = ArtistDataContext.Instance.SelectedSong.Name, 
                PathToImage = imageSouce != null ? "images/" + imageSouce.LocalPath.Split("\\").Last() : "", 
                ProducedBy = ((TextBox)dataGrid.FindName("Producer"))?.Text, 
                WrittenBy = ((TextBox)dataGrid.FindName("Writer"))?.Text,
                Status = "Awaiting Approval"
            }, ArtistDataContext.Instance.SelectedSong.Name);

            ArtistDataContext.Instance.SelectedSong = null;
            ArtistDataContext.Instance.IsSongSelected = false;
            ArtistDataContext.Instance.StatusMessage = "Awaiting Approval";

            ArtistDataContext.Instance.OnPropertyChanged("");
        }
    }
}
