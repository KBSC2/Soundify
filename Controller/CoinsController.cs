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
        private static CoinsController instance;
        public static CoinsController Instance
        {
            get
            {
                if (instance == null)
                    new CoinsController();
                return instance;
            }
            set => instance = value;
        }
        private static int Counter { get; set; }
        private UserController UserController { get; set; }
        

        private CoinsController()
        {
            Counter = 0;
            UserController = new UserController(new DatabaseContext());
        }

        public void EarnCoins(object sender, EventArgs e)
        {
            if(AudioPlayer.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                Counter += 1;

            if(Counter == 1000)
            {
                Counter = 0;
                AddCoin(CurrentUser);
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
