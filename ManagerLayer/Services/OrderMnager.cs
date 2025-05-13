using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class OrderMnager : IOrderManager
    {
        private readonly IOrderRepo orderRepo;
        public OrderMnager(IOrderRepo orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        public async Task<List<OrdersWithBookDetails>> PlaceOrderFromCart(int userId)
        {
            return await orderRepo.PlaceOrderFromCart(userId);
        }

        public async Task<List<OrdersWithBookDetails>> GetOrderDetails(int userId)
        {
            return await orderRepo.GetOrderDetails(userId);
        }
    }
}
