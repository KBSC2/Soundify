using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
