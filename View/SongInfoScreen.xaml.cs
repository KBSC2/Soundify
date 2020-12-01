using Model.DbModels;
using System.Windows;
using View.DataContexts;

namespace View
{
    /// <summary>
    /// Interaction logic for SongInfoScreen.xaml
    /// </summary>
    public partial class SongInfoScreen : Window
    {
        
        public SongInfoScreen(Song song)
        {
            InitializeComponent();
            ((SongInfoDataContext)DataContext).Song = song;
            ((SongInfoDataContext)DataContext).OnPropertyChanged();
        }
    }
}
