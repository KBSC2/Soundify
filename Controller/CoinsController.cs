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
        private int counter { get; set; }
        private UserController userController { get; set; }
        

        private CoinsController()
        {
            counter = 0;
            userController = UserController.Create(new DatabaseContext());
        }

        public void EarnCoins(object sender, EventArgs e)
        {
            if(AudioPlayer.Instance.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                counter += 1;

            if(counter == 1000)
            {
                counter = 0;
                AddCoin(UserController.CurrentUser);
            } 
        }
        public void AddCoin(User user)
        {
            user.Coins += 1;
            userController.UpdateItem(user);
        }

        public void RemoveCoin(User user)
        {
            user.Coins -= 1;
            userController.UpdateItem(user);
        }
    }
}
