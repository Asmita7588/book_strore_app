using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class CartManager : ICartManager
    {
        private readonly ICartRepo cartRepo;
        public CartManager(ICartRepo cartRepo)
        {
            this.cartRepo = cartRepo;
        }

        public async Task<CartModel> AddToCart(int userId, int bookId)
        {
            return await cartRepo.AddToCart(userId, bookId);
        }
        public async Task<bool> RemoveFromCart(int userId, int bookId)
        {
            return await cartRepo.RemoveFromCart(userId, bookId);
        }
        public async Task<CartModel> UpdateCartById(int userId, int CartId, UpdateCartModel model)
        {
            return await cartRepo.UpdateCartById(userId, CartId, model);
        }
        public async Task<List<CartModel>> GetCartItems(int userId)
        {
            return await cartRepo.GetCartItems(userId);
        }
    }
}
