using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    
    public class AdminManager : IAdminManager
    {
        private readonly IAdminRepo adminRepo;

        public AdminManager(IAdminRepo adminRepo)
        {
            this.adminRepo = adminRepo;
        }

        public AdminEntity RegisterAdmin(RegisterModel model)
        {
            return adminRepo.RegisterAdmin(model);
        }
        public bool CheckMail(string mail)
        {
            return adminRepo.CheckMail(mail);
        }

    }
}
