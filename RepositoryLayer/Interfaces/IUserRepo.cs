using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        public RegisterModel RegisterUser(RegisterModel model);
        public bool CheckMail(string mail);

        public string LoginUser(LoginModel userLoginModel);


        public ForgotPasswordModel ForgotPassword(string Email);

        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel);


        public RefreshLoginResponse AccessTokenLogin(LoginModel userLogin);

        public RefreshLoginResponse RefreshAccessToken(string refreshToken);



    }
}
