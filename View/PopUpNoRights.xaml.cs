using Model;
using Model.EventArgs;
using Soundify;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace View
{
    /// <summary>
    /// Interaction logic for PopUpNoRights.xaml
    /// </summary>
    public partial class PopUpNoRights : Window
    {
        public PopUpNoRights(string x)
        {
            InitializeComponent();
            StartTimer();
            TypeOfRight.Text = x;
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
