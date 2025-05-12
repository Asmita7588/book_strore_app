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
            public BookEntity BookDetails { get; set; }
        
    }
}
