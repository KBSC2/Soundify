using Model.DbModels;
using System.ComponentModel;
using Controller;

namespace View.DataContexts
{
    class SongInfoDataContext : INotifyPropertyChanged
    {
        public Song Song { get; set; }
        public string PathToImage => Song == null ? "../Assets/null.png" : Song.PathToImage == null ? "": FileCache.Instance.GetFile(Song.PathToImage); 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
