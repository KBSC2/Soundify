﻿using System.Windows;
using Model.DbModels;

namespace Model.EventArgs
{
    public class MenuItemRoutedEventArgs : RoutedEventArgs
    {
        public ScreenNames ScreenName { get; set; }
        public Playlist Playlist { get; set; }
    }
}
