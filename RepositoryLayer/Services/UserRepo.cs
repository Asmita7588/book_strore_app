using System;
using System.Linq;
using RepositoryLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Jwt;
using Microsoft.EntityFrameworkCore;

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

        public RegisterModel RegisterUser(RegisterModel model)
        {
            try
            {
                var user = new UserEntity
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    Role = "user"
                };
                user.Password = EncodePasswordToBase64(model.Password);
                context.Users.Add(user);
                context.SaveChanges();
                return new RegisterModel
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                };

            }
            catch (Exception ex) {
                throw new Exception("Error User registration" + ex.Message);
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
                var user = context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email && u.Password == EncodePasswordToBase64(userLoginModel.Password));
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
                    user.Password = EncodePasswordToBase64(resetPasswordModel.ConfirmPassword);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) {

                throw new Exception($"Eror in reset password: {ex.Message}");
            }
        }


        public RefreshLoginResponse AccessTokenLogin(LoginModel userLogin)
        {
            try {
                var user = context.Users.FirstOrDefault(u => u.Email == userLogin.Email && u.Password == EncodePasswordToBase64(userLogin.Password));
                if (user == null) return null;



                var accessToken = jwtFile.GenerateToken(user.Email, user.UserId, user.Role);
                var refreshToken = Guid.NewGuid().ToString();

                var refreshEntry = new TokenEntity
                {
                    UserId = user.UserId,
                    Role = user.Role,
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
            catch(Exception ex)
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

                var user = context.Users.FirstOrDefault(u => u.UserId == token.UserId);
                if (user == null) return null;

                var newAccessToken =jwtFile.GenerateToken(user.Email, user.UserId, token.Role);
                

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
