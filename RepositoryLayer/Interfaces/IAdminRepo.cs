using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
   public interface IAdminRepo
    {
        public RegisterModel RegisterAdmin(RegisterModel model);

        public bool CheckMail(string mail);

        public string LoginAdmin(LoginModel adminLoginModel);

        public ForgotPasswordModel ForgotPassword(string Email);

        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel);
    }
}
