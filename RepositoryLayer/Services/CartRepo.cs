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
    public class CartRepo : ICartRepo
    {

        private readonly BookStoreContext context;
        private readonly IConfiguration configuration;
       

        public CartRepo(BookStoreContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
           
        }


        public async Task<CartModel> AddToCart(int userId, int bookId)
        {
            try
            {
                var user = await context.Users.FindAsync(userId);
                var book = await context.Books.FindAsync(bookId);

                if (user == null || book == null)
                {
                    throw new Exception("Invalid user or book.");
                }

                var existingCartItem = await context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += 1;
                    existingCartItem.Price = (decimal)book.Price;
                }
                else
                {
                    existingCartItem = new CartEntity
                    {
                        UserId = userId,
                        BookId = bookId,
                        Quantity = 1,
                        Price = (decimal)book.Price
                    };

                    await context.Carts.AddAsync(existingCartItem);
                }

                await context.SaveChangesAsync();

                return new CartModel
                {
                    CartId = existingCartItem.CartId,
                    UserId = userId,
                    BookId = bookId,
                    Quantity = existingCartItem.Quantity,
                    Price = existingCartItem.Price,
                    IsPurchased = false,
                    FullName = user.FullName,
                    UserEmail = user.Email,
                    
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in AddToCart: {ex.Message}");
            }
        }

        public async Task<bool> RemoveFromCart(int userId, int bookId)
        {
            try
            {
                var cartItem = await context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

                if (cartItem != null)
                {
                    context.Carts.Remove(cartItem);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in RemoveFromCart: {ex.Message}");
            }
        }

        public async Task<CartModel> UpdateCartById(int userId, int CartId, UpdateCartModel model)
        {
            var user = await context.Users.FindAsync(userId);
            var book = await context.Books.FindAsync(model.BookId);
            var cartItem = await context.Carts
                .FirstOrDefaultAsync(c => c.CartId == CartId && c.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity = model.Quantity;
                cartItem.Price = cartItem.Book.Price;

                context.Carts.Update(cartItem);
                await context.SaveChangesAsync();

                return new CartModel
                {
                    UserId = cartItem.UserId,
                    BookId = cartItem.BookId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    FullName = user.FullName,
                };
            }

            return null;
        }


        public async Task<List<CartModel>> GetCartItems(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            var cartItems = await context.Carts
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return cartItems.Select(cart => new CartModel
            {
                UserId = cart.UserId,
                FullName =user.FullName, 
                BookId = cart.BookId,
                Quantity = cart.Quantity,
                Price = cart.Price,
                UserEmail = user.Email,
            }).ToList();
        }



    }
}
