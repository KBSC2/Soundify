using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.Enums;

namespace View.DataContexts
{
    public class ArtistDataContext : INotifyPropertyChanged
    {
        private static ArtistDataContext _instance;
        public static ArtistDataContext Instance => _instance ??= new ArtistDataContext();
        private static ArtistController artistController = ArtistController.Create(new DatabaseContext());

        public TagLib.File SelectedSong { get; set; }
        public BitmapImage SongImage { get; set; }

        public string StatusMessage { get; set; }

        public bool IsSongSelected { get; set; }

        public string ArtistName => ArtistController.Create(new DatabaseContext())
            .GetList().FirstOrDefault(a => a.UserID == UserController.CurrentUser.ID)
            ?.ArtistName;

        
        public bool ArtistHasSongPending { get; set; } 

        public ArtistDataContext()
        {
            ArtistHasSongPending = SongController.Create(new DatabaseContext())
                .GetList().Where(s => s.Artist == artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID) && s.Status == SongStatus.AwaitingApproval)
                .ToList().Count > 0;

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
