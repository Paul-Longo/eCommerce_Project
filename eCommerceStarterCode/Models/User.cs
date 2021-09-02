using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace eCommerceStarterCode.Models
{
    public class User : IdentityUser
    {
        public User()

        {
            ShoppingCarts = new HashSet<ShoppingCart>();



        }

        public int UserId { get; set; }
        public string Name {get; set;}
        public string ShoppingCart { get; set; }
        public string Review { get; set; }
        public string Password { get; set; }
       

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
