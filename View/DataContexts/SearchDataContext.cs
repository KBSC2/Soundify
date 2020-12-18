using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    class SearchDataContext : INotifyPropertyChanged
    {
        private static SearchDataContext _instance;

        public static SearchDataContext Instance => _instance ??= new SearchDataContext();

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> SearchTerms { get; set; } = new List<string>();
        
        public List<Playlist> SearchPlaylists => PlaylistController.Create(DatabaseContext.Instance)
            .SearchPlayListOnString(SearchTerms);
        
        public List<Album> SearchAlbums => AlbumController.Create(DatabaseContext.Instance)
                .SearchAlbumListOnString(SearchTerms).ToList();

        public List<Playlist> AllPlaylists => PlaylistController.Create(DatabaseContext.Instance)
            .GetActivePlaylists(UserController.CurrentUser);

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}