﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using RepositoryLayer.Jwt;
using RepositoryLayer.Migrations;



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

        public RegisterModel RegisterAdmin(RegisterModel model)
        {



            try
            {
                var admin = new AdminEntity
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    Role = "admin"
                };
                admin.Password = EncodePasswordToBase64(model.Password);
                context.Admins.Add(admin);
                context.SaveChanges();
                return new RegisterModel
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                };

            }
            catch (Exception ex) {

                throw new Exception("Error in Admin registration" + ex.Message);

            }

        }

        private string EncodePasswordToBase64(string password)
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
                var admin = context.Admins.FirstOrDefault(u => u.Email == adminLoginModel.Email && u.Password == EncodePasswordToBase64(adminLoginModel.Password));
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

                    
                    admin.Password = EncodePasswordToBase64(resetPasswordModel.ConfirmPassword);

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

        public RefreshLoginResponse AccessTokenLogin(LoginModel userLogin)
        {
            try
            {
                var admin = context.Admins.FirstOrDefault(u => u.Email == userLogin.Email && u.Password == EncodePasswordToBase64(userLogin.Password));
                if (admin == null) return null;



                var accessToken = jwtFile.GenerateToken(admin.Email, admin.UserId, admin.Role);
                var refreshToken = Guid.NewGuid().ToString();

                var refreshEntry = new Entity.TokenEntity
                {
                    UserId = admin.UserId,
                    Role = admin.Role,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
                };

                context.RefreshTokens.Add(refreshEntry);
                context.SaveChanges();

                return new RefreshLoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,

                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Eror in access Token: {ex.Message}");
            }

        }

        public RefreshLoginResponse RefreshAccessToken(string refreshToken)
        {
            try
            {
                var token = context.RefreshTokens.FirstOrDefault(t =>
                    t.RefreshToken == refreshToken && t.RefreshTokenExpiry > DateTime.UtcNow);
                if (token == null) return null;

                var admin = context.Admins.FirstOrDefault(u => u.UserId == token.UserId);
                if (admin == null) return null;

                var newAccessToken = jwtFile.GenerateToken(admin.Email, admin.UserId, token.Role);


                return new RefreshLoginResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = token.RefreshToken,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while refreshing access token: {ex.Message}");
            }
        }




    }
}
