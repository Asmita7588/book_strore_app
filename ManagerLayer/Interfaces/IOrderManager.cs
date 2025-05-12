using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IOrderManager
    {
        Task<List<OrdersWithBookDetails>> PlaceOrderFromCart(int userId);
    }
}
