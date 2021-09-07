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
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult GetAllCarts()
        {
            var carts = _context.Carts;
            return Ok(carts);
        }

        [HttpGet("{id}"), Authorize]
        public IActionResult GetCartbyId(int id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            Cart cart = _context.Carts.Find(id);
            if (cart == null || currentUser == null)
            {
                return NotFound();
            }
            else if (cart.UserId != currentUser.Id)
            {
                return Unauthorized();
            }
            return Ok(cart);
        }

        [HttpGet("user/{userId}"), Authorize]
        public IActionResult GetCartbyUser(string id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            User user = _context.Users.Find(id);
            if (user == null || currentUser == null)
            {
                return NotFound();
            }
            else if (user != currentUser)
            {
                return Unauthorized();
            }
            var userCart = _context.Carts.Where(x => x.User == user);
            return Ok(userCart);
        }

        [HttpPost, Authorize]
        public IActionResult NewCart([FromBody] Cart value)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            _context.Carts.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpPut, Authorize]
        public IActionResult UpdateCart([FromBody] Cart value)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            Cart cartToChange = _context.Carts.Find(value.CartId);
            if (cartToChange == null || currentUser == null)
            {
                return NotFound();
            }
            else if (cartToChange.UserId != currentUser.Id)
            {
                return Unauthorized();
            }
            cartToChange.UserId = value.UserId;
            cartToChange.ProductId = value.ProductId;
            cartToChange.Quantity = value.Quantity;
            _context.Update(cartToChange);
            _context.SaveChanges();
            return Ok(value);
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteCart(int id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            Cart cartToDelete = _context.Carts.Find(id);
            if (cartToDelete == null || currentUser == null)
            {
                return NotFound();
            }
            else if (cartToDelete.UserId != currentUser.Id)
            {
                return Unauthorized();
            }
            _context.Carts.Remove(cartToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
