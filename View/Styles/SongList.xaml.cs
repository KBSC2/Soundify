using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Soundify;
using View.DataContexts;

namespace View.Resources
{
    public partial class SongContextMenuInfo : ResourceDictionary
    {
        public void ListViewItem_RightClickAddQueue(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;

            AudioPlayer.Instance.AddSongToSongQueue(song);
            
            SongListDataContext.Instance.OnPropertyChanged();
        }
        private void ListViewItem_RightClickSongInfo(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;

            SongInfoScreen songInfoScreen = new SongInfoScreen(song);
            songInfoScreen.Show();

            SongListDataContext.Instance.OnPropertyChanged();
        }
        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {

            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList, songInfo.Index - 1);
            SongListDataContext.Instance.OnPropertyChanged();
        }
        public void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;
            var playlist = MainWindow.CurrentPlayList;
            PlaylistSongController.Create(new DatabaseContext()).RemoveFromPlaylist(song.ID, playlist.ID);

            SongListDataContext.Instance.OnPropertyChanged("");
            PlaylistDataContext.Instance.OnPropertyChanged("");
        }
        private void MenuItem_LeftClick(object sender, MouseButtonEventArgs e)
        {
            var playlist = ((Playlist)((MenuItem)sender).DataContext);
            var song = ((SongInfo)((MenuItem)((MenuItem)sender).Tag).DataContext).Song;

            var playlistSongController = PlaylistSongController.Create(new DatabaseContext());
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);

            SongListDataContext.Instance.OnPropertyChanged();
        }
    }
}
