using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllShoppingCarts()
        {
            var shoppingCarts = _context.ShoppingCarts;
            return Ok(shoppingCarts);
        }
        [HttpGet("{id}")]
        public IActionResult GetShoppingCartById(int id)
        {
            var shoppingCart = _context.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            return Ok(shoppingCart);
        }
        [HttpPost]
        public IActionResult Post([FromBody] ShoppingCart value)
        {
            _context.ShoppingCarts.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShoppingCart(int id)
        {
            var shoppingcartToDelete = _context.ShoppingCarts.Find(id);
            if (shoppingcartToDelete == null)
            {
                return NotFound();
            }
            _context.ShoppingCarts.Remove(shoppingcartToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}