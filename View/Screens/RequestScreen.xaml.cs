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

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for RequestScreen.xaml
    /// </summary>
    public partial class RequestScreen : ResourceDictionary
    {
        public static RequestScreen RequestScreenInstance { get; set; }
        private List<Request> artistRequestsList = RequestController.Create(new DatabaseContext()).GetArtistRequests();
        public RequestScreen()
        {
            RequestScreenInstance = this;
            InitializeComponent();
        }
        public void AddArtistRequests()
        {
            foreach (var request in artistRequestsList)
            {
                Expander expander = new Expander();
                expander.Header = $"{request.ArtistName} would like to become an artist";
                Grid grid = new Grid();

                var textblock = new TextBlock() { Text = request.ArtistReason };
                textblock.SetValue(Grid.RowProperty, 0);
                textblock.SetValue(Grid.ColumnSpanProperty, 2);
                grid.Children.Add(textblock);

                var approveButton = new Button() { Content = "Approve" };
                approveButton.SetValue(Grid.RowProperty, 1);
                approveButton.SetValue(Grid.ColumnProperty, 0);
                grid.Children.Add(approveButton);


                var declineButton = new Button() { Content = "Decline" };
                declineButton.SetValue(Grid.RowProperty, 1);
                declineButton.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(declineButton);

                ((Grid)FindName("RequestScreen"))?.Children.Add(grid);
            }
        }
    }
}