using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Model.EventArgs
{
    public class MenuItemRoutedEventArgs : RoutedEventArgs
    {
        public ScreenNames ScreenName { get; set; }
    }
}
