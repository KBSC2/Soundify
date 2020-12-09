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
        private UserController userController;

        private CoinsController()
        {
            counter = 0;
            userController = UserController.Create(new DatabaseContext()); //TODO: if coinsController will be tested this has to be variable.
        }

        /**
         * determines the timing of which coins need to be added
         *
         * @return void
         */
        public void EarnCoins(object sender, EventArgs e)
        {
            if (AudioPlayer.Instance.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                counter++;

            if (counter == 10)
            {
                counter = 0;
                AddCoins(userController.GetItem(UserController.CurrentUser.ID));
            }
        }

        /**
         * adds coins to the currentUser's account
         *
         * @param user currentUser
         * @param coins number of coins that need to be added
         *
         * @return void
         */
        public void AddCoins(User user, int coins = 1)
        {
            user.Coins += coins;
            userController.UpdateItem(user);
        }

        /**
          * removes coins to the currentUser's account
          *
          * @param user currentUser
          * @param coins number of coins that need to be removed
          *
          * @return void
          */
        public void RemoveCoins(User user, int coins = 1)
        {
            user.Coins -= coins;
            userController.UpdateItem(user);
        }
    }
}