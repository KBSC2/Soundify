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

        public TagLib.File SelectedSong { get; set; }
        public BitmapImage SongImage { get; set; }

        public string StatusMessage { get; set; }

        public bool IsSongSelected { get; set; }

        public string ArtistName => ArtistController.Create(new DatabaseContext())
            .GetList().ContinueWith(res =>
                res.Result.FirstOrDefault(a => a.UserID == UserController.CurrentUser.ID)?.ArtistName).Result;


        public List<SongInfo> UploadedSongs => new List<SongInfo>();
            /*SongInfo.ConvertSongListToSongInfo(ArtistController.Create(new DatabaseContext())
            .GetSongsForArtist(UserController.CurrentUser.ID).Result);*/

        public bool ArtistHasSongPending { get; set; } 

        public ArtistDataContext()
        {
            /*var list = await ConfiguredTaskAwaitable ArtistController.Create(new DatabaseContext())
                .GetSongsForArtist(UserController.CurrentUser.ID);


            ArtistHasSongPending = ArtistController.Create(new DatabaseContext())
                .GetSongsForArtist(UserController.CurrentUser.ID)
                .ContinueWith(res => res.Result.Any(s => s.Status == SongStatus.AwaitingApproval)).Result;*/

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
