using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class CartModel
    {
            public int CartId { get; set; }
            public int UserId { get; set; }
            public int BookId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public bool IsPurchased { get; set; }
            public decimal TotalPrice => Quantity * Price;
            public string FullName { get; set; }
            public string UserEmail { get; set; }
        

    }
}
