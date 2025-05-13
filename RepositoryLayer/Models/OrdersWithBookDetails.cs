using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Models
{
    public class OrdersWithBookDetails
    {

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; }
        public string UserEmail { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string BookImage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
