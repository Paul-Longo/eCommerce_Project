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

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products;
            return Ok(products);
        }

        // <baseurl>/api/examples/user
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

        [HttpGet("category/{categoryId}")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var categoryToFind = _context.Categories.Find(categoryId);
            var products = _context.Products.Where(x => x.Category == categoryToFind);

            return Ok(products);
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] Product value)
        {
            Product productToChange = _context.Products.Find(value.ProductId);
            if (productToChange != null)
            {
                return NotFound();
            }
            productToChange.CategoryId = value.CategoryId;
            productToChange.Description = value.Description;
            productToChange.Name = value.Name;
            productToChange.Price = value.Price;
            productToChange.UserId = value.UserId;
            _context.Update(productToChange);
            _context.SaveChanges();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult NewProduct([FromBody] Product value)
        {
            _context.Products.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = _context.Products.Find(id);
            if (productToDelete == null)
            {
                return NotFound();
            }
            _context.Products.Remove(productToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}