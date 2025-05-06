using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CommonLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Jwt;

namespace RepositoryLayer.Services
{
    public class UserRepo :IUserRepo
    {

        private readonly BookStoreContext context;
        private readonly IConfiguration configuration;
        private readonly JwtFile jwtFile;

        public UserRepo(BookStoreContext context, IConfiguration configuration, JwtFile jwtFile)
        {
            this.context = context;
            this.configuration = configuration;
            this.jwtFile = jwtFile;
        }

        public UserEntity RegisterUser(RegisterModel model)
        {
            UserEntity user = new UserEntity();
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.MobileNumber = model.MobileNumber;
            user.Password = EncodePasswordToBase6(model.Password);;
            this.context.Users.Add(user);
            context.SaveChanges();
            return user;
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
                var result = this.context.Users.FirstOrDefault(x => x.Email == mail);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex) {

                throw new Exception($"Email does not exists: {ex.Message}");
            }
        }


        public string LoginUser(LoginModel userLoginModel)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email && u.Password == EncodePasswordToBase6(userLoginModel.Password));
                if (user != null)
                {

                    var token = jwtFile.GenerateToken(user.Email, user.UserId, user.Role);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception($"User not found: {ex.Message}");
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

            }catch (Exception ex)
            {

                throw new Exception($"Eror in forgot password: {ex.Message}");

            }
        }
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            try
            {
                UserEntity user = context.Users.ToList().Find(user => user.Email == Email);

                if (CheckMail(user.Email))
                {
                    user.Password = EncodePasswordToBase6(resetPasswordModel.ConfirmPassword);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) {

                throw new Exception($"Email does not exists: {ex.Message}");
            }
        }





    }
}
