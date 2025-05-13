using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Entity
{
    public class CartEntity
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }  

        [ForeignKey("Book")]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Decimal Price { get; set; }

        public bool IsPurchased { get; set; }

        // Navigation properties
        public virtual UserEntity User { get; set; }
        public virtual BookEntity Book { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * Price;


        

    }
}
