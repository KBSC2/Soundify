using System;
using Controller.DbControllers;
using Model.Database.Contexts;

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
        }

        /**
         * Determines the timing of which coins need to be added
         *
         * @return void
         */
        public void EarnCoins(object sender, EventArgs e)
        {
            if (AudioPlayer.Instance.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                counter++;

            if (counter == 100)
            {
                var userController = UserController.Create(new DatabaseContext());
                UserController.CurrentUser.Coins = userController.AddCoins(userController.GetItem(UserController.CurrentUser.ID)).Coins;
                counter = 0;
            }
        }
    }
}