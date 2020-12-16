using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
using Soundify;
using View.DataContexts;

namespace View.Screens
{
    public partial class SongAlterationScreen : ResourceDictionary
    {
        public SongAlterationScreen()
        {
            InitializeComponent();
        }

        public void Safe_Button_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = (Grid)((Button)sender).Parent;

            var song = SongAlterationDataContext.Instance.Song;

            var songName = ((TextBox)dataGrid.FindName("Title"))?.Text;

            if (songName != null && songName == "")
            {
                MessageBox.Show("Title must be set", "Error", MessageBoxButton.OK);
                return;
            }

            var imageSource = ((BitmapImage)((Image)dataGrid.FindName("Image"))?.Source)?.UriSource;

            if (imageSource != null && imageSource.LocalPath != song.PathToImage)
            {
                FileTransfer.Create(new DatabaseContext()).UploadFile(imageSource.LocalPath, "images/" +
                    imageSource.LocalPath.Split("\\").Last());

                song.PathToImage = "images/" + imageSource.LocalPath.Split("\\").Last();
            }

            song.Name = songName;
            song.Description = dataGrid.FindName("Description") != null
                ? ((TextBox)dataGrid.FindName("Description"))?.Text
                : null;
            song.ProducedBy = dataGrid.FindName("Producer") != null
                ? ((TextBox)dataGrid.FindName("Producer"))?.Text
                : null;
            song.WrittenBy = dataGrid.FindName("Writer") != null ? ((TextBox)dataGrid.FindName("Writer"))?.Text : null;

            SongController.Create(new DatabaseContext()).UpdateItem(song);

            SongAlterationDataContext.Instance.OnPropertyChanged("");

            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.SongListScreen);
        }

        public void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.SongListScreen);
        }

        public void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            SongController.Create(new DatabaseContext()).DeleteSong(SongAlterationDataContext.Instance.Song);

            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.SongListScreen);
        }

        public void UploadSongImageButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog { Filter = "Files|*.jpg;*.jpeg;*.png;" };

            if (fileDialog.ShowDialog() != true) return;

            SongAlterationDataContext.Instance.ImageSource = new BitmapImage(new Uri(fileDialog.FileName));

            SongAlterationDataContext.Instance.OnPropertyChanged("");
        }
    }
}
