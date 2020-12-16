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
using System.Linq;

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
            var songList = SongListDataContext.Instance.SongInfoList.Select(x => x.Song).ToList();
            AudioPlayer.Instance.PlayAlbum(songList);
        }
    }
}
