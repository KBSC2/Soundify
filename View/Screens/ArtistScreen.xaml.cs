using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Win32;
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
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".mp3";
            fileDialog.Filter = "MP3 Files (.mp3)|*.mp3";
            if (fileDialog.ShowDialog() == true)
            {
                ArtistDataContext.Instance.SelectedSongPath = fileDialog.FileName;
            }
        }
    }
}
