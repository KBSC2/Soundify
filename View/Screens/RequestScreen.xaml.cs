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
        private RequestController requestController;

        public RequestScreen()
        {
            InitializeComponent();
            requestController = RequestController.Create(new DatabaseContext());
        }

        private void Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button) sender).Tag;

            requestController.ApproveUser(requestID);

            RequestDatacontext.Instance.OnPropertyChanged("");
        }

        private void Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button)sender).Tag;
            
            requestController.DeclineUser(requestID);

            RequestDatacontext.Instance.OnPropertyChanged("");
        }
    }
}