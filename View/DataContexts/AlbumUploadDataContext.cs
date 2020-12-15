using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using TagLib;
using View.ListItems;

namespace View.DataContexts
{
    public class AlbumUploadDataContext : INotifyPropertyChanged
    {

        private static AlbumUploadDataContext instance;

        public static AlbumUploadDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new AlbumUploadDataContext();
                return instance;
            }
        }

        public List<TagLib.File> AlbumFiles { get; set; } = new List<File>();

        public ObservableCollection<AlbumSongInfo> AlbumSongInfos { get; set; }

        public BitmapImage AlbumImage { get; set; }
        public bool AreSongsSelected { get; set; }
        public string StatusMessage { get; set; }

        
        public string ArtistName => ArtistController.Create(DatabaseContext.Instance)
            .GetList().FirstOrDefault(a => a.UserID == UserController.CurrentUser.ID)
            ?.ArtistName;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
