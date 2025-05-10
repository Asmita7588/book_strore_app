using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRepo
    {
        Task<CartModel> AddToCart(int userId, int bookId);
        Task<bool> RemoveFromCart(int userId, int bookId);
       Task<CartModel> UpdateCartById(int userId, int CartId, UpdateCartModel model);
        Task<List<CartModel>> GetCartItems(int userId);
    }
}
