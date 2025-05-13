using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly BookStoreContext context;
        public CustomerRepo(BookStoreContext bookStoreContext)
        {
            this.context = bookStoreContext;
        }
        public async Task<CustomerDetails> AddCustomerDetailsAsync(int userId, CustomerDetailModel model)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var customer = new CustomerDetails
                {
                    FullName = model.FullName,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    Type = model.Type,
                    UserId = userId
                };

                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex) { 
                throw new Exception($"error in customer registration : {ex.Message}");
            }
        }

        public async Task<List<CustomerResponse>> GetAllCustomerDetailsAsync()
        {
            try
            {
                var customers = await context.Customers
                    .Select(c => new CustomerResponse
                    {
                        CustomerId = c.CustomerId,
                        UserId = c.UserId,
                        FullName = c.FullName,
                        Mobile = c.Mobile,
                        Address = c.Address,
                        City = c.City,
                        State = c.State,
                        Type = c.Type
                    })
                    .ToListAsync();

                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting customer details: {ex.Message}");
            }
        }




    }
}
