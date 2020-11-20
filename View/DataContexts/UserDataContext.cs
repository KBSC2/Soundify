using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace View.DataContexts
{
    public class UserDataContext
    {
        public string UsernameLogin { get; set; } = "Username";
        public string PasswordLogin { get; set; } = "Password";
        public string UsernameRegister { get; set; } = "Username";
        public string EmailRegister { get; set; } = "Email";
        public string PasswordRegister { get; set; } = "Password";
        public string PasswordConfirmRegister { get; set; } = "Password";
    }
}
