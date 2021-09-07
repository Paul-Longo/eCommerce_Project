using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // <baseurl>/api/product
        // Openly available.
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products;
            return Ok(products);
        }


        // <baseurl>/api/product/<id>
        // Openly available.
        [HttpGet("{id}")]
        public IActionResult GetSelectedProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // <baseurl>/api/product/category/<categoryId>
        // Openly available.
        [HttpGet("category/{categoryId}")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var categoryToFind = _context.Categories.Find(categoryId);
            var products = _context.Products.Where(x => x.Category == categoryToFind);

            return Ok(products);
        }

        // <baseurl>/api/product/seller/<sellerId>
        // Openly available.
        [HttpGet("seller/{sellerId}")]
        public IActionResult GetProductsBySeller(string sellerId)
        {
            User seller = _context.Users.Find(sellerId);
            if (seller == null)
            {
                return NotFound();
            }
            var sellersProducts = _context.Products.Where(x => x.Seller == seller);
            return Ok(sellersProducts);
        }

        // <baseurl>/api/product
        // Only available to seller.
        [HttpPut, Authorize]
        public IActionResult UpdateProduct([FromBody] Product value)
        {
            // Get current user and requested product to change
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            Product productToChange = _context.Products.Find(value.ProductId);

            // Check for real values
            if (productToChange == null || currentUser == null)
            {
                return NotFound();
            }
            // check that currentUser is seller
            else if (productToChange.Seller != currentUser)
            {
                return Unauthorized();
            }

            // Update info
            productToChange.CategoryId = value.CategoryId;
            productToChange.Description = value.Description;
            productToChange.Name = value.Name;
            productToChange.Price = value.Price;
            productToChange.UserId = value.UserId;
            _context.Update(productToChange);
            _context.SaveChanges();
            return Ok(productToChange);
        }


        // <baseurl>/api/product
        // Openly available.
        [HttpPost, Authorize]
        public IActionResult NewProduct([FromBody] Product value)
        {
            _context.Products.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }


        // <baseurl>/api/product/<id>
        // Only available to seller.
        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteProduct(int id)
        {
            // Get current user and requested product to delete
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var productToDelete = _context.Products.Find(id);

            // Check for real values
            if (productToDelete == null || currentUser == null)
            {
                return NotFound();
            }
            // Verify currentUser is seller
            else if (productToDelete.Seller != currentUser)
            {
                return Unauthorized();
            }

            // Delete and save
            _context.Products.Remove(productToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
