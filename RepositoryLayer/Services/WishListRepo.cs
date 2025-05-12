using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using System.Threading.Tasks;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RepositoryLayer.Services
{
    public class WishListRepo :IWishListRepo
    {
        private readonly BookStoreContext context;
        private readonly IConfiguration configuration;
        private readonly JwtFile jwtFile;

        public WishListRepo(BookStoreContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
          
        }

        public async Task<BookModel> AddToWishlist(int userId, int bookId)
        {
            try
            {
               
                var user = await context.Users.FindAsync(userId);
                var book = await context.Books.FindAsync(bookId);

                if (user == null || book == null)
                {
                    throw new Exception("Invalid user or book ID.");
                }

                
                var existingWishlist = await context.WishList
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId && !w.IsPurchased);

                if (existingWishlist != null)
                {
                    throw new Exception("Book already exists in wishlist.");
                }

                var wishlist = new WishListEntity
                {
                    UserId = userId,
                    BookId = bookId,
                    IsPurchased = false
                };

                await context.WishList.AddAsync(wishlist);
                await context.SaveChangesAsync();

                var bookModel = new BookModel
                {   
                    BookName = book.BookName,
                    Author = book.Author,
                    Description = book.Description,
                    Price = book.Price
                };

                return bookModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding to wishlist: " + ex.Message);
            }
        }

        public async Task<List<BookModel>> GetWishlistByUser(int userId)
        {
            try
            {
                var wishlistItems = await context.WishList
                    .Include(w => w.Book)
                    .Where(w => w.UserId == userId && !w.IsPurchased)
                    .ToListAsync();

                if (wishlistItems == null || !wishlistItems.Any())
                {
                    return new List<BookModel>();
                }

                var bookList = wishlistItems.Select(w => new BookModel
                {
                    BookId = w.Book.BookId,
                    BookName = w.Book.BookName,
                    Author = w.Book.Author,
                    Description = w.Book.Description,
                    Price = w.Book.Price
                }).ToList();

                return bookList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving wishlist: " + ex.Message);
            }
        }

        public async Task<BookModel> RemoveFromWishlist(int userId, int bookId)
        {
            try
            {
                var item = await context.WishList
                    .Include(w => w.Book)
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId && !w.IsPurchased);

                if (item == null)
                    return null;

                var book = item.Book;

                context.WishList.Remove(item);
                await context.SaveChangesAsync();

                return new BookModel
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    Author = book.Author,
                    Description = book.Description,
                    Price = book.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while removing wishlist: " + ex.Message);
            }
        }


        public async Task<WishListEntity> UpdateWishlist(WishListUpdateModel model, int userId)
        {
            try
            {
                var wishlist = await context.WishList
                    .Include(w => w.Book)
                    .FirstOrDefaultAsync(w => w.WishListId == model.WishlistId && w.UserId == userId);

                if (wishlist == null)
                    return null;

                if (model.BookId.HasValue)
                    wishlist.BookId = model.BookId.Value;

                //if (model.IsPurchased.HasValue)
                //    wishlist.IsPurchased = model.IsPurchased.Value;

                await context.SaveChangesAsync();
                return wishlist;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating wishlist: " + ex.Message);
            }

        }


    }
}
