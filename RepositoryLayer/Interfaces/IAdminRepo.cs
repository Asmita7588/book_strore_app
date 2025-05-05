using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
   public interface IAdminRepo
    {
        public AdminEntity RegisterAdmin(RegisterModel model);
        public bool CheckMail(string mail);

        public string LoginAdmin(LoginModel adminLoginModel);
    }
}
