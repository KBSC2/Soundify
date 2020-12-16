using Model.DbModels;
using System.Windows;
using Controller.DbControllers;
using Model.Database.Contexts;
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

            // Makes screen in universal font
            Style = (Style)FindResource(typeof(Window));

            ((SongInfoDataContext)DataContext).Song = song;
            ((SongInfoDataContext) DataContext).ArtistName =
                ArtistController.Create(new DatabaseContext()).GetItem(song.Artist).ArtistName;
            ((SongInfoDataContext)DataContext).OnPropertyChanged();
        }
    }
}
