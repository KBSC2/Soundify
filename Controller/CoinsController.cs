using System;
using Controller.DbControllers;
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
                    instance = new CoinsController();
                return instance;
            }
        }

        private int counter;

        private CoinsController()
        {
            counter = 0;
        }

        public void EarnCoins(object sender, EventArgs e)
        {
            if (AudioPlayer.Instance.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                counter++;

            if(counter == 1000)
            {
                counter = 0;
                AddCoins();
            } 
        }

        public void AddCoins(int coins = 1)
        {
            var userController = UserController.Create(new DatabaseContext());
            var user = userController.GetItem(UserController.CurrentUser.ID);
            user.Coins += coins;
            userController.UpdateItem(user);
        }

        public void RemoveCoins(User user, int coins = 1)
        {
            var userController = UserController.Create(new DatabaseContext());
            user.Coins -= coins;
            userController.UpdateItem(user);
        }
    }
}
