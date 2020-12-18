using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Castle.Core.Internal;
using Controller;
using Controller.DbControllers;
using Microsoft.Win32;
using Model.Database.Contexts;
using Model.MailTemplates;
using View.DataContexts;
using View.ListItems;

namespace View.Screens
{
    public partial class AlbumUploadScreen : ResourceDictionary
    {
        public AlbumUploadScreen()
        {
            InitializeComponent();
        }

        private void UploadAlbumImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog { Filter = "Files|*.jpg;*.jpeg;*.png;" };

            if (fileDialog.ShowDialog() != true) return;

            AlbumUploadDataContext.Instance.AlbumImage = new BitmapImage(new Uri(fileDialog.FileName));

            AlbumUploadDataContext.Instance.OnPropertyChanged("");
        }

        private void UploadAlbumSongsButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog() { DefaultExt = ".mp3", Filter = "MP3 Files (.mp3)|*.mp3" };
            fileDialog.Multiselect = true;
            fileDialog.Title = "Select the songs for the album";
            if (fileDialog.ShowDialog() != true) return;

            var albumFiles = new List<TagLib.File>();
            fileDialog.FileNames.ToList().ForEach(fs => albumFiles.Add(TagLib.File.Create(fs)));
            AlbumUploadDataContext.Instance.AlbumFiles = albumFiles;

            var albumSongs = new ObservableCollection<AlbumSongInfo>();
            AlbumUploadDataContext.Instance.AlbumFiles.ForEach(f => albumSongs.
                Add(new AlbumSongInfo(f.Tag.Title, f.Properties.Duration, f)));
            AlbumUploadDataContext.Instance.AlbumSongInfos = albumSongs;

            AlbumUploadDataContext.Instance.AreSongsSelected = true;
            AlbumUploadDataContext.Instance.StatusMessage = "Uploading Songs";
            AlbumUploadDataContext.Instance.OnPropertyChanged("");
        }

        private void Textbox_LostFocus_EditVarialble(object sender, RoutedEventArgs e)
        {
            var song = (((TextBox) sender).DataContext as AlbumSongInfo);

            var albumSongInfo = AlbumUploadDataContext.Instance.AlbumSongInfos.First(s => song != null && s.File.Equals(song.File));
            var i = AlbumUploadDataContext.Instance.AlbumSongInfos.IndexOf(albumSongInfo);
            AlbumUploadDataContext.Instance.AlbumSongInfos[i] = song;
        }

        private void ListViewItem_ButtonClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            var song = (sender as Button)?.DataContext as AlbumSongInfo;
            AlbumUploadDataContext.Instance.AlbumSongInfos.Remove(song);
            AlbumUploadDataContext.Instance.AlbumFiles.Remove(song?.File);
            if (AlbumUploadDataContext.Instance.AlbumSongInfos.IsNullOrEmpty())
            {
                AlbumUploadDataContext.Instance.AlbumFiles = null;
                AlbumUploadDataContext.Instance.AlbumSongInfos = null;
                AlbumUploadDataContext.Instance.AreSongsSelected = false;
                AlbumUploadDataContext.Instance.StatusMessage = "";
            }
            AlbumUploadDataContext.Instance.OnPropertyChanged("");
        }

        private void Button_ConfirmUploadAlbum(object sender, RoutedEventArgs e)
        {
            var titleTextBox = (TextBox)((Button)sender).Tag;
            var dataGrid = (Grid)((Button)sender).CommandParameter;
            var genre = dataGrid.FindName("Genre") != null
                ? ((TextBox)dataGrid.FindName("Genre"))?.Text
                : null;
            var description = dataGrid.FindName("Description") != null
                ? ((TextBox) dataGrid.FindName("Description"))?.Text
                : null;
            var artistName = AlbumUploadDataContext.Instance.ArtistName;

            if (titleTextBox.Text == "")
            {
                MessageBox.Show("Title must be set", "Error", MessageBoxButton.OK);
                return;
            }
            if (AlbumUploadDataContext.Instance.AlbumImage == null)
            {
                MessageBox.Show("Album image must be set", "Error", MessageBoxButton.OK);
                return;
            }

            var image = AlbumUploadDataContext.Instance.AlbumImage.UriSource;

            FileTransfer.Create(DatabaseContext.Instance).UploadFile(image?.LocalPath, "images/" +
                image?.LocalPath.Split("\\").Last());

            AlbumController.Create(DatabaseContext.Instance).UploadAlbum(AlbumUploadDataContext.Instance.AlbumSongInfos, image, titleTextBox.Text, description, artistName,genre);
            AlbumUploadDataContext.Instance.AlbumFiles = null;
            AlbumUploadDataContext.Instance.AlbumSongInfos = null;
            AlbumUploadDataContext.Instance.AreSongsSelected = false;
            AlbumUploadDataContext.Instance.StatusMessage = "Album is succesfully uploaded";
            AlbumUploadDataContext.Instance.OnPropertyChanged("");
            
            var emailController = new EmailController();
            var email = new MailAlbumApprovalTemplate(new MailAddress("info.soundify@gmail.com"), artistName, titleTextBox.Text);
            emailController.SendEmail(email, "info.soundify@gmail.com");
        }
    }
}
