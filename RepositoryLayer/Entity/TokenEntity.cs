using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Entity
{
    public class TokenEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserEntity User { get; set; } // navigation property

        [Required]
        public string Role { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime RefreshTokenExpiry { get; set; }
    }

}

