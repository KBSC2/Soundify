using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Controller.DbControllers;
using Microsoft.Win32;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Soundify;

namespace View.DataContexts
{
    public class ArtistDataContext : INotifyPropertyChanged
    {
        private static ArtistDataContext _instance;
        public static ArtistDataContext Instance => _instance ??= new ArtistDataContext();

        public TagLib.File SelectedSong { get; set; }
        public BitmapImage SongImage { get; set; }

        public string StatusMessage { get; set; }

        public bool IsSongSelected { get; set; }

        public string ArtistName => new ArtistController(new DatabaseContext())
            .GetList().FirstOrDefault(a => a.UserID == DataContext.Instance.CurrentUser.ID)
            ?.ArtistName;

        public List<SongInfo> UploadedSongs => SongInfo.ConvertSongListToSongInfo(new SongController(new DatabaseContext()).GetList()
            .Where(s => s.Artist == new ArtistController(new DatabaseContext()).GetList()
                .FirstOrDefault(a => a.UserID == DataContext.Instance.CurrentUser.ID)?.ArtistName).ToList());


        public bool ArtistHasSongPending { get; set; } 

        public ArtistDataContext()
        {
            ArtistHasSongPending = new SongController(new DatabaseContext())
                .GetList().Where(s => s.Artist == ArtistName && s.Status == SongStatus.AwaitingApproval).ToList().Count > 0;

            StatusMessage = ArtistHasSongPending ? "Awaiting Approval" : "Waiting for song";

            OnPropertyChanged("");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
