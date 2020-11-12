using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View;
using Controller;
using Model.Data;
using Model.Models;

namespace Soundify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DatabaseContext Context { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Context = new DatabaseContext();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            this.Close();
            win2.Show();
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            //PlaylistMenu win3 = new PlaylistMenu();
            // temporarily until playlist menu is implemented
            var win3 = new Playlist();
            this.Close();
            win3.Show();
            
        }
    }
}
