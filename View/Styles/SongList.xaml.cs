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
        private void ListViewItem_RightClickAddQueue(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.AddSongToSongQueue(((SongInfo)((MenuItem)sender).DataContext).Song);

            SongListDataContext.Instance.OnPropertyChanged();
        }

        private void ListViewItem_RightClickSongInfo(object sender, RoutedEventArgs e)
        {
            new SongInfoScreen(((SongInfo)((MenuItem)sender).DataContext).Song).Show();

            SongListDataContext.Instance.OnPropertyChanged();
        }

        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            if (((ListViewItem)sender).Content is SongInfo songInfo)
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
            AlbumSongListDataContext.Instance.Album = ((SongInfo)((MenuItem)sender).DataContext).Song.Album;

            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.AlbumSongListScreen,
            });
        }


        private void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            PlaylistSongController.Create(DatabaseContext.Instance).RemoveFromPlaylist(MainWindow.CurrentPlayList, ((SongInfo)((MenuItem)sender).DataContext).Song.ID);

            SongListDataContext.Instance.OnPropertyChanged("");
            PlaylistDataContext.Instance.OnPropertyChanged("");
        }

        private void MenuItem_LeftClick(object sender, MouseButtonEventArgs e)
        {
            var playlistSongController = PlaylistSongController.Create(DatabaseContext.Instance);
            playlistSongController.AddSongToPlaylist((Playlist)((MenuItem)sender).DataContext, ((SongInfo)((MenuItem)((MenuItem)sender).Tag).DataContext).Song.ID);

            SongListDataContext.Instance.OnPropertyChanged();
        }

        private void ListViewItem_ButtonClick_EditSong(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.DataContext is SongInfo songInfo)) return;
            
            SongAlterationDataContext.Instance.SetSong(songInfo.Song);
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.SongAlterationScreen);
        }

        private void SelectedItem_Selected(object sender, RoutedEventArgs e )
        {
            var songInfo = (((ListViewItem) sender).Content as SongInfo);
            SongListDataContext.Instance.SelectedSongInfo = songInfo;
        }
    }
}
