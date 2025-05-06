using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Jwt;

namespace RepositoryLayer.Services
{
    public class AdminRepo :IAdminRepo
    {

        private readonly BookStoreContext context;
        private readonly IConfiguration configuration;
        private readonly JwtFile jwtFile;

        public AdminRepo(BookStoreContext context, IConfiguration configuration, JwtFile jwtFile)
        {
            this.context = context;
            this.configuration = configuration;
            this.jwtFile = jwtFile;
        }

        public AdminEntity RegisterAdmin(RegisterModel model)
        {
            try
            {
                AdminEntity admin = new AdminEntity();
                admin.FullName = model.FullName;
                admin.Email = model.Email;
                admin.MobileNumber = model.MobileNumber;
                admin.Password = EncodePasswordToBase6(model.Password); ;
                this.context.Admins.Add(admin);
                context.SaveChanges();
                return admin;
            }
            catch (Exception ex) {

                throw new Exception("Error in Admin registration" + ex.Message);

            }
        }

        private string EncodePasswordToBase6(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public bool CheckMail(string mail)
        {
            try
            {
                var result = this.context.Admins.FirstOrDefault(x => x.Email == mail);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                throw new Exception("Error  while checking mail" + ex.Message);
            }
        }

        public string LoginAdmin(LoginModel adminLoginModel)
        {
            try
            {
                var admin = context.Admins.FirstOrDefault(u => u.Email == adminLoginModel.Email && u.Password == EncodePasswordToBase6(adminLoginModel.Password));
                if (admin != null)
                {

                    var token = jwtFile.GenerateToken(admin.Email, admin.UserId, admin.Role);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {

               throw ex;
            }
        }



        public ForgotPasswordModel ForgotPassword(string Email)
        {
            try
            {
                UserEntity user = context.Users.ToList().Find(user => user.Email == Email);
                ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
                forgotPassword.Email = user.Email;
                forgotPassword.UserId = user.UserId;
                forgotPassword.Token = jwtFile.GenerateToken(user.Email, user.UserId, user.Role);
                return forgotPassword;

            }
            catch (Exception ex)
            {

                throw new Exception($"Eror in forgot password: {ex.Message}");

            }
        }


        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            try
            {
                AdminEntity admin = context.Admins.ToList().Find(admin => admin.Email == Email);

                if (CheckMail(admin.Email))
                {
                    admin.Password = EncodePasswordToBase6(resetPasswordModel.ConfirmPassword);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error in Reset password: {ex.Message}");
            }
        }







    }
}
