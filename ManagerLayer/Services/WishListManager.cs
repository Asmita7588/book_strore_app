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
    public class WishListManager : IWishListManager
    {
        private readonly IWishListRepo wishListRepo;
        public WishListManager(IWishListRepo wishListRepo)
        {
            this.wishListRepo = wishListRepo;
        }
        public async Task<BookModel> AddToWishlist(int userId, int bookId)
        {
            return await wishListRepo.AddToWishlist(userId, bookId);
        }
        public async Task<List<BookModel>> GetWishlistByUser(int userId)
        {
            return await wishListRepo.GetWishlistByUser(userId);
        }

        public async Task<BookModel> RemoveFromWishlist(int userId, int bookId)
        {
            return await wishListRepo.RemoveFromWishlist(userId, bookId);
        }

        public async Task<WishListEntity> UpdateWishlist(WishListUpdateModel model, int userId)
        {
            return await wishListRepo.UpdateWishlist(model, userId);
        }
    }
}
