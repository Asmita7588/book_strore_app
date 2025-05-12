using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class OrderRepo : IOrderRepo
    {
        private readonly BookStoreContext context;
        public OrderRepo(BookStoreContext context) {
            this.context = context;
        }

        public async Task<List<OrdersWithBookDetails>> PlaceOrderFromCart(int userId)
        {
            var cartItems = await context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Book)
                .ToListAsync();
            if (!cartItems.Any())
                return null;

            var ordersWithDetails = new List<OrdersWithBookDetails>();

            foreach (var item in cartItems)
            {
                var order = new OrderEntity
                {
                    UserId = userId,
                    BookId = item.BookId,
                    Price = item.Price,
                    OrderDate = DateTime.UtcNow
                };

                await context.OrderSummary.AddAsync(order);
                //item.IsPurchased = true;

                var orderWithBookDetails = new OrdersWithBookDetails
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    BookId = order.BookId,
                    Price = order.Price,
                    OrderDate = order.OrderDate,
                    BookDetails = new BookEntity
                    {
                        BookId = item.Book.BookId,
                        BookName = item.Book.BookName,
                        Author = item.Book.Author,
                        Price = item.Book.Price,
                        Description = item.Book.Description,
                        BookImage = item.Book.BookImage
                    }
                };

                ordersWithDetails.Add(orderWithBookDetails);
            }

            context.Carts.RemoveRange(cartItems);
            await context.SaveChangesAsync();

            return ordersWithDetails;
        }
    }

}

    
