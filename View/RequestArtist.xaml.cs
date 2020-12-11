﻿using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.MailTemplates;
using View.DataContexts;

namespace View
{
    /// <summary>
    /// Interaction logic for RequestArtist.xaml
    /// </summary>
    public partial class RequestArtist : Window
    {
        public RequestArtist()
        {
            InitializeComponent();
        }

        private void Confirm_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirm_Button_Click(sender, new RoutedEventArgs());
            }
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var artistName = this.ArtistName.Text;
            var artistReason = this.ArtistReason.Text;

            var emailController = new EmailController();
            var email =  new MailArtistVerification(new MailAddress("info.soundify@gmail.com"), artistName, artistReason);

            switch (email)
            {
                case RequestArtistResults.Success:
                    {
                        emailController.SendEmail(email, "info.soundify@gmail.com");

                        var request = new Request()
                        {
                            ArtistName = artistName,
                            ArtistReason = artistReason,
                            UserID = UserController.CurrentUser.ID,
                            RequestType = RequestType.Artist,
                            SongID = null
                        };


                        var userController = UserController.Create(new DatabaseContext());
                        var user = userController.GetItem(UserController.CurrentUser.ID);
                        user.RequestedArtist = true;
                        userController.UpdateItem(user);

                        var createRequest = RequestController.Create(new DatabaseContext());
                        createRequest.CreateItem(request);

                        this.Close();
                    
                        this.ArtistName.Text = "";
                        this.ArtistReason.Text = "";
                        break;
                    }
                case RequestArtistResults.ArtistNameNotFound:
                    {
                        this.Error.Content = "No Artist name has been entered";
                        break;
                    }
                case RequestArtistResults.ReasonNotFound:
                    {
                        this.Error.Content = "You haven't given a reason to become an artist";
                        break;
                    }

                    MessageBox.Show("Request Artist is confirmed!");
            SettingsDataContext.Instance.OnPropertyChanged("");
        }


        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
