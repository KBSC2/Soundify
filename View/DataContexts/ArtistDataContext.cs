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
        private static ArtistController artistController = ArtistController.Create(DatabaseContext.Instance);

        public TagLib.File SelectedSong { get; set; }
        public BitmapImage SongImage { get; set; }

        public string StatusMessage { get; set; }

        public bool IsSongSelected { get; set; }

        public string ArtistName => ArtistController.Create(DatabaseContext.Instance)
            .GetList().FirstOrDefault(a => a.UserID == UserController.CurrentUser.ID)
            ?.ArtistName;

        
        public bool ArtistHasSongPending { get; set; } 

        public ArtistDataContext()
        {
            ArtistHasSongPending = artistController.GetArtistFromUser(UserController.CurrentUser).Singles
                .Count(x => x.Status == SongStatus.AwaitingApproval) > 0;

            /*ArtistHasSongPending = SongController.Create(DatabaseContext.Instance)
                .GetList().Where(s => s.ArtistID == artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID) && s.Status == SongStatus.AwaitingApproval)
                .ToList().Count > 0;*/

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
