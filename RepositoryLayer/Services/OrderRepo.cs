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
        public OrderRepo(BookStoreContext context)
        {
            this.context = context;
        }

        public async Task<List<OrdersWithBookDetails>> PlaceOrderFromCart(int userId)
        {
            try
            {
                var cartItems = await context.Carts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Book)
                    .ToListAsync();
                var user = context.Users.FirstOrDefault(n => n.UserId == userId);
                if (!cartItems.Any())
                    return null;
                decimal grandTotal = cartItems?.Sum(item => item.TotalPrice) ?? 0;
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

                    item.IsPurchased = true;
                    var book = context.Books.FirstOrDefault(n => n.BookId == item.BookId);
                    var orderWithBookDetails = new OrdersWithBookDetails
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        BookId = order.BookId,
                        Price = order.Price,
                        OrderDate = order.OrderDate,
                        BookName = item.Book.BookName,
                        Author = item.Book.Author,
                        Description = item.Book.Description,
                        BookImage = item.Book.BookImage,
                        Quantity = item.Book.Quantity,
                        TotalPrice = grandTotal,

                    };
                    ordersWithDetails.Add(orderWithBookDetails);
                }
                context.Carts.RemoveRange(cartItems);
                await context.SaveChangesAsync();

                return ordersWithDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"error in order Place {ex.Message}");
            }
        }

        public async Task<List<OrdersWithBookDetails>> GetOrderDetails(int userId)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                    return null;

                var orders = await context.OrderSummary
                    .Where(o => o.UserId == userId)
                    .Include(o => o.Book)
                    .ToListAsync();

                if (!orders.Any())
                    return null;

                decimal grandTotal = orders.Sum(o => o.Price);

                var ordersWithDetails = orders.Select(order => new OrdersWithBookDetails
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    FullName = user.FullName,
                    UserEmail = user.Email,
                    BookId = order.BookId,
                    Price = order.Price,
                    OrderDate = order.OrderDate,
                    BookName = order.Book.BookName,
                    Author = order.Book.Author,
                    Description = order.Book.Description,
                    BookImage = order.Book.BookImage,
                    Quantity = order.Book.Quantity,
                    TotalPrice = grandTotal
                }).ToList();

                return ordersWithDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching order details: {ex.Message}");
            }
        }



    }

}


