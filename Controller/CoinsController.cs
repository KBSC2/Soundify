using System;
using Controller.DbControllers;
using Model.Database.Contexts;

namespace Controller
{
    public class CoinsController
    {
        private static CoinsController instance;
        public static CoinsController Instance => instance ??= new CoinsController();

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

            if (counter == 10)
            {
                var userController = UserController.Create(new DatabaseContext());
                userController.AddCoins(UserController.CurrentUser);
                counter = 0;
            }
        }
    }
}