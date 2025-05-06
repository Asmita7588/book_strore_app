using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public UserEntity RegisterUser(RegisterModel model);
        public bool CheckMail(string mail);

        public string LoginUser(LoginModel userLoginModel);

        public ForgotPasswordModel ForgotPassword(string Email);

        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel);
    }
}
