using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    
    public class UserManager : IUserManager
    {


        private readonly IUserRepo userRepo;
        public UserManager(IUserRepo userRepo) {

            this.userRepo = userRepo; 
        }
    
    
        public RegisterModel RegisterUser(RegisterModel model)
        {
            return userRepo.RegisterUser(model);
        }

        public bool CheckMail(string mail)
        {
            return userRepo.CheckMail(mail);
        }

        public string LoginUser(LoginModel userLoginModel)
        {
            return userRepo.LoginUser(userLoginModel);
        }

        public ForgotPasswordModel ForgotPassword(string Email)
        {
            return userRepo.ForgotPassword(Email);
        }

        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            return userRepo.ResetPassword(Email, resetPasswordModel);   
        }

        public RefreshLoginResponse AccessTokenLogin(LoginModel userLogin)
        {
            return userRepo.AccessTokenLogin(userLogin);
        }
        public RefreshLoginResponse RefreshAccessToken(string refreshToken)
        {
            return userRepo.RefreshAccessToken(refreshToken);   
        }
    }
}
