using Model.Enums;

namespace Model.EventArgs
{
    public class NoRightsEventArgs : System.EventArgs
    {
        public Permissions Permission { get; set; }

    }
}
