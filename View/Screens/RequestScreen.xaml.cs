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
            var request = RequestController.Create(new DatabaseContext()).GetItem(requestID);
            var userID = request.UserID;

            var userController = UserController.Create(new DatabaseContext());
            var user = userController.GetItem(userID);
            user.RoleID = 2;
            userController.UpdateItem(user);

            ArtistController.Create(new DatabaseContext()).CreateItem(new Artist(){ArtistName = request.ArtistName, UserID = userID});

            RequestController.Create(new DatabaseContext()).DeleteItem(requestID);

            //Doesn't work, fix
            //RequestDatacontext.Instance.OnPropertyChanged("");
        }

        private void Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button)sender).Tag;
            var request = RequestController.Create(new DatabaseContext()).GetItem(requestID);
            var userID = request.UserID;

            var userController = UserController.Create(new DatabaseContext());
            var user = userController.GetItem(userID);
            user.RequestedArtist = false;
            userController.UpdateItem(user);

            RequestController.Create(new DatabaseContext()).DeleteItem(requestID);

            //Doesn't work, fix
            //RequestDatacontext.Instance.OnPropertyChanged("");
        }
    }
}