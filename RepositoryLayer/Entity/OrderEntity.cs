using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class OrderEntity
    {
        
            [Key]
            public int OrderId { get; set; }

            public int UserId { get; set; }

            public int BookId { get; set; }

            public decimal Price { get; set; }

            public DateTime OrderDate { get; set; }

            public UserEntity User { get; set; }

            public BookEntity Book { get; set; }

        

    }
}
