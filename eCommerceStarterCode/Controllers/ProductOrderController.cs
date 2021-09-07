using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // <baseurl>/api/productorder
        // Only available to admins. Nobody else needs to see all POs
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult GetAllProductOrders()
        {
            var productOrders = _context.ProductOrders;
            return Ok(productOrders);
        }

        // <baseurl>/api/productorder/<id>
        // Only available to buyer or seller. 
        [HttpGet("{id}"), Authorize]
        public IActionResult GetSelectedProductOrder(int id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var productOrder = _context.ProductOrders.Find(id);
            if (productOrder == null || currentUser == null)
            {
                return NotFound();
            }
            if (!(IsBuyer(currentUser, productOrder) || IsSeller(currentUser, productOrder)))
            {
                return Unauthorized();
            }
            return Ok(productOrder);
        }

        // <baseurl>/api/productorder/order/<orderId>
        // Only available to buyer. Orders are accessible in other ways for sellers.
        [HttpGet("order/{orderId}"), Authorize]
        public IActionResult GetProductOrdersByOrder(int orderId)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var orderToFind = _context.Orders.Find(orderId);
            if (orderToFind == null || currentUser == null)
            {
                return NotFound();
            }
            if (orderToFind.User != currentUser)
            {
                return Unauthorized();
            }
            var productOrders = _context.ProductOrders.Where(x => x.Order == orderToFind);

            return Ok(productOrders);
        }

        // <baseurl>/api/productorder/seller/<sellerId>
        // Only available to seller. 
        [HttpGet("seller/{sellerId}")]
        public IActionResult GetProductOrdersBySeller(string sellerId)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            User seller = _context.Users.Find(sellerId);
            if (seller == null || currentUser == null)
            {
                return NotFound();
            }
            if (seller != currentUser)
            {
                return Unauthorized();
            }
            var sellersProductOrders = _context.ProductOrders.Include(po => po.Product).Where(po => po.Product.Seller == seller);
            return Ok(sellersProductOrders);
        }

        // <baseurl>/api/productorder
        // Available to anybody logged in.
        [HttpPost, Authorize]
        public IActionResult NewProductOrder([FromBody] ProductOrder value)
        {
            _context.ProductOrders.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        // <baseurl>/api/productorder
        // Only available to buyer or seller. 
        [HttpPut]
        public IActionResult UpdateProductOrder([FromBody] ProductOrder value)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            ProductOrder productOrderToChange = _context.ProductOrders.Find(value.ProductOrderId);
            if (productOrderToChange == null || currentUser == null)
            {
                return NotFound();
            }
            if (!(IsBuyer(currentUser, productOrderToChange) || IsSeller(currentUser, productOrderToChange)))
            {
                return Unauthorized();
            }
            productOrderToChange.ProductId = value.ProductId;
            productOrderToChange.OrderId = value.OrderId;
            productOrderToChange.Quantity = value.Quantity;
            _context.Update(productOrderToChange);
            _context.SaveChanges();
            return Ok(value); 
        }

        // <baseurl>/api/productorder/seller/<sellerId>
        // Only available to buyer or seller. 
        [HttpDelete("{id}")]
        public IActionResult DeleteProductOrder(int id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            ProductOrder productOrderToDelete = _context.ProductOrders.Find(id);
            if (productOrderToDelete == null || currentUser == null)
            {
                return NotFound();
            }
            if (!(IsBuyer(currentUser, productOrderToDelete) || IsSeller(currentUser, productOrderToDelete)))
            {
                return Unauthorized();
            }
            if (productOrderToDelete == null)
            {
                return NotFound();
            }
            _context.ProductOrders.Remove(productOrderToDelete);
            _context.SaveChanges();
            return Ok();
        }

        private bool IsBuyer(User user, ProductOrder po)
        {
            return po.Order.User == user;
        }

        private bool IsSeller(User user, ProductOrder po)
        {
            return po.Product.Seller == user;
        }
    }
}