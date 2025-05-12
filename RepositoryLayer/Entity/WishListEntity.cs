using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
   public class WishListEntity
   {
       
        [Key]
        public int WishListId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public bool IsPurchased { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual BookEntity Book { get; set; }
    }



}

