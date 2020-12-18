using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using View.ListItems;

namespace View.DataContexts
{
    public class RequestDatacontext : INotifyPropertyChanged
    {
        private static RequestDatacontext instance;
        public static RequestDatacontext Instance => instance ??= new RequestDatacontext();
        public List<Request> ArtistRequests => RequestController.Create(DatabaseContext.Instance).GetArtistRequests();

        public List<SongRequestInfo> SongRequests =>
            SongRequestInfo.ConvertSongRequestToSongRequestInfo(RequestController.Create(DatabaseContext.Instance)
                .GetSongRequests());


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
