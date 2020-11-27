using Controller;
using Soundify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            AudioPlayer.PlayPlaylist(MainWindow.CurrentPlayList, 1);
        }
    }
}
