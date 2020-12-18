using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.EventArgs;
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
                        var songList = SongListDataContext.Instance.SongInfoList.Select(x => x.Song).ToList();
                        AudioPlayer.Instance.PlayAlbum(songList, songList.IndexOf(songInfo.Song) - 1);
                        break;
                }
            }

            SongListDataContext.Instance.OnPropertyChanged("SongInfoList");
        }

        private void OpenAlbum_LeftClick(object sender, RoutedEventArgs e)
        {
            var songInfo = ((SongInfo) ((MenuItem)sender).DataContext);
            
                AlbumSongListDataContext.Instance.Album = songInfo.Song.Album;

                MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
                {
                    ScreenName = ScreenNames.AlbumSongListScreen,
                });
        }


        public void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;
            var playlist = MainWindow.CurrentPlayList;
            PlaylistSongController.Create(DatabaseContext.Instance).RemoveFromPlaylist(playlist, song.ID);

            SongListDataContext.Instance.OnPropertyChanged("");
            PlaylistDataContext.Instance.OnPropertyChanged("");
        }

        private void MenuItem_LeftClick(object sender, MouseButtonEventArgs e)
        {
            var playlist = ((Playlist)((MenuItem)sender).DataContext);
            var song = ((SongInfo)((MenuItem)((MenuItem)sender).Tag).DataContext).Song;

            var playlistSongController = PlaylistSongController.Create(DatabaseContext.Instance);
            playlistSongController.AddSongToPlaylist(playlist, song.ID);

            SongListDataContext.Instance.OnPropertyChanged();
        }

        public void ListViewItem_ButtonClick_EditSong(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.DataContext is SongInfo songInfo)) return;
            
            SongAlterationDataContext.Instance.SetSong(songInfo.Song);
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.SongAlterationScreen);
        }
    }
}