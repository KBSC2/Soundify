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
        private static ArtistDataContext instance;
        public static ArtistDataContext Instance => instance ??= new ArtistDataContext();

        public TagLib.File SelectedSong { get; set; }
        public BitmapImage SongImage { get; set; }

        public string StatusMessage { get; set; }

        public bool IsSongSelected { get; set; }

        public string ArtistName => ArtistController.Create(DatabaseContext.Instance)
            .GetArtistFromUser(UserController.CurrentUser)?.ArtistName;

        public bool ArtistHasSongPending { get; set; } 

        public ArtistDataContext()
        {
            ArtistHasSongPending = ArtistController.Create(DatabaseContext.Instance).GetArtistFromUser(UserController.CurrentUser)?.Singles
                .Count(x => x.Status == SongStatus.AwaitingApproval) > 0;

            StatusMessage = ArtistHasSongPending ? "Awaiting Approval" : "Waiting for song";

            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
