using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for RequestScreen.xaml
    /// </summary>
    public partial class RequestScreen : ResourceDictionary
    {
        public RequestScreen()
        {
            InitializeComponent();
        }

        private void Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button) sender).Tag;

            ApproveUser(requestID, new DatabaseContext());

            RequestDatacontext.Instance.OnPropertyChanged("");
        }

        public async void ApproveUser(int requestID, IDatabaseContext databaseContext)
        {
            var request = await RequestController.Create(databaseContext).GetItem(requestID);

            var userID = request.UserID;

            ArtistController.Create(databaseContext).MakeArtist(userID);

            RequestController.Create(databaseContext).DeleteItem(requestID);
        }

        private void Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button)sender).Tag;
            
            DeclineUser(requestID, new DatabaseContext());

            RequestDatacontext.Instance.OnPropertyChanged("");
        }

        public void DeclineUser(int requestID, IDatabaseContext databaseContext)
        {
            RequestController.Create(databaseContext).GetItem(requestID).ContinueWith(res =>
            {
                var request = res.Result;
                var userID = request.UserID;

                var userController = UserController.Create(databaseContext);
                userController.GetItem(userID).ContinueWith(x =>
                {
                    var user = x.Result;
                    user.RequestedArtist = false;
                    userController.UpdateItem(user);
                    RequestController.Create(databaseContext).DeleteItem(requestID);
                });
            });
        }
    }
}