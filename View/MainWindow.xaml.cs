using System.Windows;
using View;
using Model.Data;

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
