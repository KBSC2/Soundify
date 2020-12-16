using System.Collections.Generic;
using System.ComponentModel;
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

        public static SearchDataContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SearchDataContext();
                return _instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> SearchTerms { get; set; } = new List<string>();
        
        public List<Playlist> SearchPlaylists => PlaylistController.Create(DatabaseContext.Instance).SearchPlayListOnString(SearchTerms, UserController.CurrentUser.ID);

        public List<Playlist> AllPlaylists => PlaylistController.Create(DatabaseContext.Instance).GetActivePlaylists(UserController.CurrentUser.ID);

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}