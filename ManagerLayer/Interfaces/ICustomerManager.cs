using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface ICustomerManager
    {
        Task<CustomerDetails> AddCustomerDetailsAsync(int userId, CustomerDetailModel model);
        Task<List<CustomerResponse>> GetAllCustomerDetailsAsync();
    }
}
