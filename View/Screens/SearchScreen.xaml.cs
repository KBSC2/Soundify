using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model.DbModels;
using Model.Enums;
using Model.EventArgs;
using Soundify;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for SearchScreen.xaml
    /// </summary>
    public partial class SearchScreen : ResourceDictionary
    {
        public SearchScreen()
        {
            InitializeComponent();
        }

        public void PlaylistRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var selectedPlaylist = (Playlist)listViewItem.Content;

            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.PlaylistScreen,
                Playlist = selectedPlaylist
            });
        }
        public void AlbumRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var selectedAlbum = (Album)listViewItem.Content;
            AlbumSongListDataContext.Instance.Album = selectedAlbum;

            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.AlbumSongListScreen,
            });
        }
    }
}
