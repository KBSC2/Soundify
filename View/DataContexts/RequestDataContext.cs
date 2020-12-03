﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class RequestDatacontext : INotifyPropertyChanged
    {
        private static RequestDatacontext _instance;
        public static RequestDatacontext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RequestDatacontext();
                return _instance;
            }
        }
        public List<Request> ArtistRequests => RequestController.Create(new DatabaseContext()).GetArtistRequests();
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
