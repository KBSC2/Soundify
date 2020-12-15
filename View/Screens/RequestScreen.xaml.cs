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