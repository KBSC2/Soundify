using Controller;
using Model.DbModels;
using Soundify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using View.DataContexts;

namespace View.Screens
{
    partial class QueueScreen : ResourceDictionary
    {
        public QueueScreen()
        {
            this.InitializeComponent();
        }

        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            var index = AudioPlayer.NextInPlaylist.IndexOf(songInfo.Song);

            AudioPlayer.PlayPlaylist(MainWindow.CurrentPlayList, index-1);
            QueueDataContext.Instance.OnPropertyChanged();
        }
    }
}
