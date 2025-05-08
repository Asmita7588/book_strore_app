using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Models
{
    public class RegisterModel
    {
        
        [Column("FUllName", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "FirstName required")]
        [RegularExpression("^[a-zA-Z]+([ '-][a-zA-Z]+)*$", ErrorMessage = "Only letters are accepted")]
        public string FullName { get; set; }

        
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        [Column("Email", TypeName = "varchar(100)")]
        public string Email { get; set; }

        
        [Column("Password", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Password required")]
        [MinLength(6, ErrorMessage = "min 6 characters required")]
        public string Password { get; set; }

        [Required]
        [Column("MobileNumber", TypeName = "varchar(15)")]
        public string MobileNumber { get; set; }

    }
}
