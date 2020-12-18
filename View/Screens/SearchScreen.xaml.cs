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
            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.PlaylistScreen,
                Playlist = (Playlist)((ListViewItem)sender).Content
            });
        }
        public void AlbumRow_Click(object sender, MouseButtonEventArgs e)
        {
            AlbumSongListDataContext.Instance.Album = (Album)((ListViewItem)sender).Content;

            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.AlbumSongListScreen,
            });
        }
    }
}
