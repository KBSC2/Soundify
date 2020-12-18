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
            requestController.ApproveUser(requestController.GetItem((int)((Button)sender).Tag));

            RequestDatacontext.Instance.OnPropertyChanged();
        }

        private void Artist_Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            requestController.DeclineUser(requestController.GetItem((int)((Button)sender).Tag));

            RequestDatacontext.Instance.OnPropertyChanged();
        }


        private void Song_Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            requestController.ApproveSong(requestController.GetItem((int)((Button)sender).Tag));

            RequestDatacontext.Instance.OnPropertyChanged();
        }

        private void Song_Decline_Button_Click(object sender, RoutedEventArgs e)
        {
            requestController.DeclineSong(requestController.GetItem((int)((Button)sender).Tag));

            RequestDatacontext.Instance.OnPropertyChanged();
        }
    }
}