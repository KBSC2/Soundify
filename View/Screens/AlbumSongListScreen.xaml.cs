using System.Windows;
using Controller;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using View.DataContexts;
using Model.EventArgs;
using Soundify;
using System;
using Model.Enums;

namespace View.Screens
{
    partial class AlbumSongListScreen : ResourceDictionary
    {
        
        public AlbumSongListScreen()
        {
            this.InitializeComponent();
        }

        private void Play_Album_Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: vincent dingetje
        }
    }
}
