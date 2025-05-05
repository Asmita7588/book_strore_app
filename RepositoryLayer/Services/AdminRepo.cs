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
    public class AdminRepo :IAdminRepo
    {

        private readonly BookStoreContext context;
        private readonly IConfiguration configuration;

        public AdminRepo(BookStoreContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public AdminEntity RegisterAdmin(RegisterModel model)
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
            var result = this.context.Admins.FirstOrDefault(x => x.Email == mail);
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
