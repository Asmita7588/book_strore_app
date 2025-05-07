using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
   public interface IAdminRepo
<<<<<<< Updated upstream
    {
        public AdminEntity RegisterAdmin(RegisterModel model);
=======
   {
        public RegisterModel RegisterAdmin(RegisterModel model);
>>>>>>> Stashed changes
        public bool CheckMail(string mail);

        public string LoginAdmin(LoginModel adminLoginModel);
    }
}
