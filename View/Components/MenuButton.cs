﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Model;
using Model.EventArgs;
using Soundify;

namespace View.Components
{
    public class MenuButton : Button
    {

        public ScreenNames ScreenName { get; set; }

        public MenuButton()
        {
            VerticalAlignment = VerticalAlignment.Top;
            Background = (Brush) new BrushConverter().ConvertFrom("#FF424D82");
            Foreground = (Brush) new BrushConverter().ConvertFrom("#FFFFFF");
            BorderBrush = null;
        }

        protected override void OnClick()
        {
            base.OnClick();
            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs() {ScreenName = this.ScreenName});
        }
    }
}
