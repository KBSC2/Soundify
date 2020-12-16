using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Enums
{
    public enum NewUserInfo
    {
        EmailChanged,
        UsernameChanged,
        BothChanged,
        EmailExists,
        UsernameExists,
        BothExists,
        Empty
    }
}
