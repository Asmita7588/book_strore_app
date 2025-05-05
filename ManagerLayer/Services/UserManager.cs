using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    
    public class UserManager : IUserManager
    {


        private readonly IUserRepo userRepo;
        public UserManager(IUserRepo userRepo) {

            this.userRepo = userRepo; 
        }
    
    
        public UserEntity RegisterUser(RegisterModel model)
        {
            return userRepo.RegisterUser(model);
        }

        public bool CheckMail(string mail)
        {
            return userRepo.CheckMail(mail);
        }
    }
}
