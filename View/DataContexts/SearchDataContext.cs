using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Annotations;
using Model.Data;
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
        
        public List<SongInfo> SearchSongs => SongInfo.ConvertSongListToSongInfo(new SongController(new DatabaseContext()).SearchSongsOnString(SearchTerms));
        
        public List<Playlist> SearchPlaylists => new PlaylistController(new DatabaseContext()).SearchPlayListOnString(SearchTerms, DataContext.Instance.CurrentUser.ID);

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
