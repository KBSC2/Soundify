using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

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
        public List<Request> ArtistRequests => RequestController.Create(DatabaseContext.Instance).GetArtistRequests();
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
