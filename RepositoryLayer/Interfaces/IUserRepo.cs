using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        public UserEntity RegisterUser(RegisterModel model);

        public bool CheckMail(string mail);


    }
}
