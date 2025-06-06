﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface IWishListManager
    {
        Task<BookModel> AddToWishlist(int userId, int bookId);
        Task<List<BookModel>> GetWishlistByUser(int userId);
        Task<BookModel> RemoveFromWishlist(int userId, int bookId);
        Task<WishListEntity> UpdateWishlist(WishListUpdateModel model, int userId);





    }
}
