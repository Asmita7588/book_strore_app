using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class RefreshTokenModel
    {
      
        [Required(ErrorMessage = "Refresh token is required.")]
        public string RefreshToken { get; set; }
    

    }
}
