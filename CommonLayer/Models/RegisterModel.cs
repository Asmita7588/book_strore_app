using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Models
{
    public class RegisterModel
    {
        
        [Column("FUllName", TypeName = "varchar(100)")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Column("Email", TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column("Password", TypeName = "varchar(100)")]
        public string Password { get; set; }

        [Required]
        [Column("MobileNumber", TypeName = "varchar(15)")]
        public string MobileNumber { get; set; }

    }
}
