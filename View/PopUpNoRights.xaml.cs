using Controller.DbControllers;
using Model;
using Model.Database.Contexts;
using Model.Enums;
using Model.EventArgs;
using Soundify;
using System;
using System.Windows;
using System.Windows.Threading;

namespace View
{
    /// <summary>
    /// Interaction logic for PopUpNoRights.xaml
    /// </summary>
    public partial class PopUpNoRights : Window
    {
        public PopUpNoRights(Permissions permission)
        {
            InitializeComponent();
            StartTimer();

            var rpController = RolePermissionsController.Create(new DatabaseContext());

            //Decides what text should show up in the popup message
            switch (permission)
            {
                case Permissions.SongShuffle:
                    {
                        TypeOfRight.Text = "You don't have the shuffle feature\nMore features available in the shop";
                        break;
                    }
                case Permissions.SongNext:
                    {
                        TypeOfRight.Text = "You don't have the feature\nto skip to the next song\nMore features available in the shop";
                        break;
                    }
                case Permissions.SongPrev:
                    {
                        TypeOfRight.Text = "You don't have the feature\nto go to the previous song\nMore features available in the shop";
                        break;
                    }
                case Permissions.SongLoop:
                    {
                        TypeOfRight.Text = "You don't have the feature\nto loop songs\nMore features available in the shop";
                        break;
                    }
                case Permissions.PlaylistLimit:
                    {
                        var max = rpController.GetPermissionValueCount(UserController.CurrentUser, permission);
                        TypeOfRight.Text = $"You have reached the\n{max} playlists that you can make\nMore features available in the shop";
                        break;
                    }
                case Permissions.PlaylistRename:
                    {
                        TypeOfRight.Text = "You don't have the feature\nto rename a playlists\nMore features available in the shop";
                        break;
                    }
                case Permissions.PlaylistSongsLimit:
                    {
                        var max = rpController.GetPermissionValueCount(UserController.CurrentUser, permission);
                        TypeOfRight.Text = $"You have reached the maximum of\n{max} songs that you can add to the playlist\nMore features available in the shop";
                        break;
                    }
                case Permissions.AccountUsernameChange:
                    {
                        TypeOfRight.Text = "You don't have the feature\nto change your username\nMore features available in the shop";
                        break;
                    }
            }
        }
        public void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += Window_Close ;
            timer.Start();
        }
        public void Window_Close(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= Window_Close;
            Close();
        }
        public void Shop_Close_Window_Click(object sender, EventArgs e)
        {
            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs() { ScreenName = ScreenNames.QueueScreen });
            Close();
        }
    }
}
