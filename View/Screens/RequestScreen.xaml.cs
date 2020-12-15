using System.Windows;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
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
            requestController = RequestController.Create(DatabaseContext.Instance);
        }

        private void Artist_Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button) sender).Tag;

            requestController.ApproveUser(requestID);

            RequestDatacontext.Instance.OnPropertyChanged("");
        }

        private void Artist_Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button)sender).Tag;
            
            requestController.DeclineUser(requestID);

            RequestDatacontext.Instance.OnPropertyChanged("");
        }


        private void Song_Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button)sender).Tag;

            requestController.ApproveSong(requestID);

            RequestDatacontext.Instance.OnPropertyChanged("");
        }

        private void Song_Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            var requestID = (int)((Button)sender).Tag;

            requestController.DeclineSong(requestID);

            RequestDatacontext.Instance.OnPropertyChanged("");
        }
    }
}