using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Model;
using Model.EventArgs;
using Soundify;

namespace View.Components
{
    public class MenuButton : PermissionButton
    {

        public ScreenNames ScreenName { get; set; }

        public MenuButton()
        {
            VerticalAlignment = VerticalAlignment.Top;
            Background = (Brush) new BrushConverter().ConvertFrom("#FF2D7AC8");
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
