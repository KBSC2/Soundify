﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Soundify;
using View.DataContexts;

namespace View.Resources
{
    public partial class SongContextMenuInfo : ResourceDictionary
    {
        public void ListViewItem_RightClickAddQueue(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo) ((MenuItem) sender).DataContext).Song;

            AudioPlayer.Instance.AddSongToSongQueue(song);

            SongListDataContext.Instance.OnPropertyChanged();
        }

        private void ListViewItem_RightClickSongInfo(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo) ((MenuItem) sender).DataContext).Song;

            SongInfoScreen songInfoScreen = new SongInfoScreen(song);
            songInfoScreen.Show();

            SongListDataContext.Instance.OnPropertyChanged();
        }

        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem) sender;
            if (listViewItem.Content is SongInfo songInfo)
            {
                switch (SongListDataContext.Instance.ScreenName)
                {
                    case ScreenNames.PlaylistScreen:
                        AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList, songInfo.Index - 1);
                        break;
                    case ScreenNames.SearchScreen:
                        AudioPlayer.Instance.PlaySong(songInfo.Song);
                        break;
                    case ScreenNames.ArtistScreen:
                        AudioPlayer.Instance.PlaySong(songInfo.Song);
                        break;
                    case ScreenNames.SongListScreen:
                        AudioPlayer.Instance.PlaySong(songInfo.Song);
                        break;
                    case ScreenNames.AlbumSongListScreen:
                        //TODO: vincent album afspeel dingetje
                        break;
                }
            }

            SongListDataContext.Instance.OnPropertyChanged();
        }

        public void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo) ((MenuItem) sender).DataContext).Song;
            var playlist = MainWindow.CurrentPlayList;
            PlaylistSongController.Create(DatabaseContext.Instance).RemoveFromPlaylist(song.ID, playlist.ID);

            SongListDataContext.Instance.OnPropertyChanged("");
            PlaylistDataContext.Instance.OnPropertyChanged("");
        }

        private void MenuItem_LeftClick(object sender, MouseButtonEventArgs e)
        {
            var playlist = ((Playlist) ((MenuItem) sender).DataContext);
            var song = ((SongInfo) ((MenuItem) ((MenuItem) sender).Tag).DataContext).Song;

            var playlistSongController = PlaylistSongController.Create(DatabaseContext.Instance);
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);

            SongListDataContext.Instance.OnPropertyChanged();
        }

        public void ListViewItem_ButtonClick_EditSong(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            var song = button.DataContext as SongInfo;
            //TODO: Open the songalteration screen from here PROJ 55-56
        }
    }
}