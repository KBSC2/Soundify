using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Controller.DbControllers;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller
{
   public class CoinsController
    {
        private static CoinsController _instance;
        public static CoinsController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CoinsController();
                return _instance;
            }
        }
        private int Counter { get; set; }
        private UserController UserController { get; set; }
        

        private CoinsController()
        {
            Counter = 0;
            UserController = UserController.Create(new DatabaseContext());
        }

        public void EarnCoins(object sender, EventArgs e)
        {
            if(AudioPlayer.Instance.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                Counter += 1;

            if(Counter == 1000)
            {
                Counter = 0;
                AddCoin(UserController.CurrentUser);
            } 
        }
        public void AddCoin(User user)
        {
            user.Coins += 1;
            UserController.UpdateItem(user);
        }

        public void RemoveCoin(User user)
        {
            user.Coins -= 1;
            UserController.UpdateItem(user);
        }
    }
}
