using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class AdminEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column("FUllName", TypeName = "varchar(100)")]
        public string FullName { get; set; }

        [Required]
        //[EmailAddress]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        [Column("Email", TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column("Password", TypeName = "varchar(100)")]
        public string Password { get; set; }

        [Required]
        [Column("MobileNumber", TypeName = "varchar(15)")]
        public string MobileNumber { get; set; }

        public string Role { get; set; } 
    }
}
