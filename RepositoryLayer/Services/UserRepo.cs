using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
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



    }
}
