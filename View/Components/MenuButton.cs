using System.Windows;
using System.Windows.Media;
using Model.Enums;
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
            Background = (Brush) new BrushConverter().ConvertFrom("#FF2F406D");
            Foreground = (Brush) new BrushConverter().ConvertFrom("#FFFFFF");
            FontWeight = FontWeights.Bold;
            FontSize = 15;
            BorderBrush = null;

            Permission = Permissions.PlaylistCreate;
        }

        protected override void OnClick()
        {
            base.OnClick();
            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs() { ScreenName = this.ScreenName });
        }
    }
}
