using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IAdminManager
    {
        public AdminEntity RegisterAdmin(RegisterModel model);

        public bool CheckMail(string mail);
        public string LoginAdmin(LoginModel adminLoginModel);

    }
}
