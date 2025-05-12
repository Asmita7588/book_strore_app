using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class WishListUpdateModel
    {
            public int WishlistId { get; set; }
            public int? BookId { get; set; }
            //public bool? IsPurchased { get; set; }

    }
}
