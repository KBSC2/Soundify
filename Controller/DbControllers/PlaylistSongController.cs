using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistSongController
    {
        private IDatabaseContext context;
        private SongController songController;
        private PlaylistController playlistController;

        private DbSet<PlaylistSong> set;

        public static PlaylistSongController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PlaylistSongController>(new object[] { context }, context);
        }

        protected PlaylistSongController(IDatabaseContext context)
        {
            this.context = context;
            set = context.PlaylistSongs;
            songController = SongController.Create(context); 
            playlistController = PlaylistController.Create(context);
        }

        public void AddSongToPlaylist(int songID, int playlistID)
        {
            ReorderSongIndexes(playlistID);

            GetSongsFromPlaylist(playlistID).ContinueWith(res =>
            {
                var x = res.Result;

                if (x.Any(x => x.SongID == songID))
                    return;

                var playlistSong = new PlaylistSong()
                {
                    PlaylistID = playlistID,
                    SongID = songID,
                    Index = set.Count(),
                    Song = songController.GetItem(songID).Result,
                    Playlist = playlistController.GetItem(playlistID).Result,
                    Added = DateTime.Now
                };

                set.Add(playlistSong);

                if (!RealDatabase()) return;

                context.Entry(playlistSong).State = EntityState.Added;
                context.SaveChanges();
            });
        }

        public void ReorderSongIndexes(int playlistId)
        {
            set.AsEnumerable()
                .Where(x => x.PlaylistID == playlistId)
                .OrderBy(x => x.Index)
                .Select((p,i) => new {song = p, index = i})
                .Where(p => p.song.Index != p.index).ToList()
                .ForEach(p =>
                {
                    p.song.Index = p.index;
                    this.UpdatePlaylistSong(p.song);
                });
        }

        public void RemoveFromPlaylist(int songId, int playlistId)
        {
            RowExists(songId, playlistId).ContinueWith(res =>
            {
                if (!res.Result)
                    throw new ArgumentOutOfRangeException();

                GetPlaylistSong(playlistId, songId).ContinueWith(ps =>
                {
                    var playlistSong = ps.Result;
                    set.Remove(playlistSong);

                    if (RealDatabase())
                    {
                        context.Entry(playlistSong).State = EntityState.Deleted;
                        context.SaveChanges();
                    }

                    ReorderSongIndexes(playlistId);
                });
            });
        }

        public virtual async Task<List<PlaylistSong>> GetList()
        {
            return await Task.Run(() => set.ToList());
        }

        public async Task<bool> RowExists(int songId, int playlistId)
        {
            return await GetList().ContinueWith(res => 
                    res.Result
                        .Where(p => p.PlaylistID == playlistId)
                        .Any(s => s.SongID == songId
                    )
               );
        }

        public async Task<List<PlaylistSong>> GetSongsFromPlaylist(int playlistId)
        {
            ReorderSongIndexes(playlistId);

            return await GetList().ContinueWith(res =>
            {
                var songs = res.Result;
                songs.ForEach(s =>
                {
                    s.Song = songController.GetItem(s.SongID).Result;
                    s.Playlist = playlistController.GetItem(s.PlaylistID).Result;
                });
                return songs;
            });
        }

        public async Task<PlaylistSong> GetPlaylistSong(int playlistId, int songId)
        {
            return await GetSongsFromPlaylist(playlistId).ContinueWith(res =>
                res.Result.First(s => s.SongID == songId)
            );

        }

        public async Task<PlaylistSong> GetPlaylistSongFromIndex(int playlistId, int index)
        {
            return await GetSongsFromPlaylist(playlistId).ContinueWith(res =>  
                res.Result.First(s => s.Index == index)
            );
        }

        public void UpdatePlaylistSong(PlaylistSong playlistSong)
        {
            set.Update(playlistSong);

            if (!RealDatabase()) return;

            context.Entry(playlistSong).State = EntityState.Modified;
            context.SaveChanges();
        }

        public bool RealDatabase()
        {
            return context is DatabaseContext;
        }
    }
}
