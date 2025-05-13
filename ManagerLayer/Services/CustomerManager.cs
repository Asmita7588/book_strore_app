using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class CustomerManager :ICustomerManager
    {
        private readonly ICustomerRepo customerRepo;
        public CustomerManager(ICustomerRepo customerRepo)
        {
            this.customerRepo = customerRepo;   
        }
        public async Task<CustomerDetails> AddCustomerDetailsAsync(int userId, CustomerDetailModel model)
        {
          return await customerRepo.AddCustomerDetailsAsync(userId, model);
        }

        public async Task<List<CustomerResponse>> GetAllCustomerDetailsAsync()
        {
            return await customerRepo.GetAllCustomerDetailsAsync();
        }
    }
}
