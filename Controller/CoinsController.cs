﻿using System;
using Controller.DbControllers;
using Model.Database.Contexts;

namespace Controller
{
    public class CoinsController
    {
        private static CoinsController instance;
        public static CoinsController Instance => instance ??= new CoinsController();
        public event EventHandler UserCoinsEarned;

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
                userController.AddCoins(UserController.CurrentUser);
                counter = 0;
                UserCoinsEarned?.Invoke(this, new EventArgs());
            }
        }
    }
}