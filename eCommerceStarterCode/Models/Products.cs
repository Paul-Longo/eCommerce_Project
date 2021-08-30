using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace eCommerceStarterCode.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Category> Categories { get; set; }