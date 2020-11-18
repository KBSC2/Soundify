using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.DbModels;
using Model;
using Model.Annotations;

namespace Model.Contexts
{
    public class PlaylistDataContext : INotifyPropertyChanged
    {
        public string PlaylistName { get; set; }
        public string Description { get; set; }
        public List<SongInfo> PlaylistItems { get; set; }
        public Playlist Playlist { get; set; }
        public List<Song> Songs { get; set; }

        public void AddPlaylistsToMenu()
        {
            PlaylistName = Playlist.Name;
            Description = Playlist.Description;

            PlaylistItems = new List<SongInfo>();

            if (Playlist.PlaylistSongs != null && Playlist.PlaylistSongs.Count != 0)
            {
                var playlistList = (List<PlaylistSong>)Playlist.PlaylistSongs;

                playlistList.ForEach(song => PlaylistItems.Add(
                    new SongInfo(song.Song.Name, song.Song.Artist, TimeSpan.FromSeconds(song.Song.Duration), song.Added)
                ));
            }

            OnPropertyChanged("");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
