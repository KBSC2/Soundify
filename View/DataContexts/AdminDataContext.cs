using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Database.Contexts;

namespace View.DataContexts
{
    public class AdminDataContext : INotifyPropertyChanged
    {
        private static AdminDataContext instance;
        public static AdminDataContext Instance => instance ??= new AdminDataContext();
        public int NumberOfRequests => RequestController.Create(DatabaseContext.Instance).GetList().Count;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
