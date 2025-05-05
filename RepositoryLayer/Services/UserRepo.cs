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

namespace RepositoryLayer.Services
{
    public class UserRepo :IUserRepo
    {

        private readonly BookStoreContext context;
        private readonly IConfiguration configuration;

        public UserRepo(BookStoreContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
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
            var result = this.context.Users.FirstOrDefault(x => x.Email == mail);
            if (result == null)
            {
                return false;
            }
            return true;
        }


        public string LoginUser(LoginModel userLoginModel)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Email == userLoginModel.Email && u.Password == EncodePasswordToBase6(userLoginModel.Password));
                if (user != null)
                {

                    var token = GenerateToken(user.Email, user.UserId, user.Role);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        private string GenerateToken(string email, int userId, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim("Role", role),
                new Claim("UserId", userId.ToString())

            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }



   





    }
}
