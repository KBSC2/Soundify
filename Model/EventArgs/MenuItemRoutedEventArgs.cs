using System.Windows;
using Model.DbModels;
using Model.Enums;

namespace Model.EventArgs
{
    /**
     * These event args are used to set a new screen in the main window
     */
    public class MenuItemRoutedEventArgs : RoutedEventArgs
    {
        public ScreenNames ScreenName { get; set; }
        public Playlist Playlist { get; set; }
    }
}
