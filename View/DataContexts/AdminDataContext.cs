using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class AdminDataContext : INotifyPropertyChanged
    {
        private static AdminDataContext _instance;
        public static AdminDataContext Instance => _instance ??= new AdminDataContext();
        public List<Request> ArtistRequests => RequestController.Create(new DatabaseContext()).GetArtistRequests();
        public int NumberOfArtistRequest => RequestController.Create(new DatabaseContext()).GetArtistRequests().Count;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
