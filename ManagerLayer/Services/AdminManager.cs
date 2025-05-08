using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepo adminRepo;

        public AdminManager(IAdminRepo adminRepo)
        {
            this.adminRepo = adminRepo;
        }

        public RegisterModel RegisterAdmin(RegisterModel model)
        {
            return adminRepo.RegisterAdmin(model);
        }
        public bool CheckMail(string mail)
        {
            return adminRepo.CheckMail(mail);
        }

        public string LoginAdmin(LoginModel adminLoginModel)
        {
            return adminRepo.LoginAdmin(adminLoginModel);
        }
        public ForgotPasswordModel ForgotPassword(string Email)
        {
            return adminRepo.ForgotPassword(Email);
        }
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            return adminRepo.ResetPassword(Email, resetPasswordModel);
        }

        public RefreshLoginResponse AccessTokenLogin(LoginModel userLogin)
        {
            return adminRepo.AccessTokenLogin(userLogin);
        }

        public RefreshLoginResponse RefreshAccessToken(string refreshToken)
        {
            return adminRepo.RefreshAccessToken(refreshToken);
        }
    }
}
