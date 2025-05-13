using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRepo
    {
        Task<List<OrdersWithBookDetails>> PlaceOrderFromCart(int userId);
         Task<List<OrdersWithBookDetails>> GetOrderDetails(int userId);
    }
}
