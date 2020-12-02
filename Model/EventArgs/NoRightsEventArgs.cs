using Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Model.EventArgs
{
    public class NoRightsEventArgs : System.EventArgs
    {
        public Permissions Permission { get; set; }

    }
}
