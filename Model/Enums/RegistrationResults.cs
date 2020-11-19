using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Enums
{
    public enum RegistrationResults
    {
        EmailTaken,
        PasswordNoMatch,
        Succeeded,
        PasswordNotStrongEnough,
    }
}
