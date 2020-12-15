using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using View.ListItems;

namespace View.DataContexts
{
    public class RequestDatacontext : INotifyPropertyChanged
    {
        private static RequestDatacontext instance;
        public static RequestDatacontext Instance
        {
            get
            {
                if (instance == null)
                    instance = new RequestDatacontext();
                return instance;
            }
        }

        public List<Request> ArtistRequests => RequestController.Create(new DatabaseContext()).GetArtistRequests();

        public List<SongRequestInfo> SongRequests =>
            SongRequestInfo.ConvertSongRequestToSongRequestInfo(RequestController.Create(new DatabaseContext())
                .GetSongRequests());

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

    }
}
