using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.EventArgs;
using Soundify;
using View.DataContexts;
using View.ListItems;

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
            var selectedAlbum = (AlbumInfo)listViewItem.Content;
            AlbumSongListDataContext.Instance.Album = selectedAlbum.Album;

            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.AlbumSongListScreen,
            });
        }
    }
}
