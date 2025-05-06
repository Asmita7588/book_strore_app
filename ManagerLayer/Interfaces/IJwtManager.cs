using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface IJwtManager
    {
        public string GenerateToken(string email, int userId, string role);
    }
}
