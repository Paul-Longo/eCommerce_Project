using eCommerceStarterCode.Data;
using eCommerceStarterCode.DataTransferObjects;
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
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // <baseurl>/api/order
        // Only available to admins. Nobody else needs to see all orders.
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult GetAllOrders()
        {
            var products = _context.Orders;
            return Ok(products);
        }

        // <baseurl>/api/order/<id>
        // Only available to purchaser.
        [HttpGet("{id}")]
        public IActionResult GetSelectedOrder(int id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var order = _context.Orders.Find(id);
            if (order == null || currentUser == null)
            {
                return NotFound();
            }
            if (!(order.User == currentUser))
            {
                return Unauthorized();
            }
            return Ok(order);
        }

        // <baseurl>/api/order/customer/<customerId>
        // Only available to the customer.
        [HttpGet("customer/{customerId}"), Authorize]
        public IActionResult GetOrdersByCustomer(string customerId)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var customerToFind = _context.Users.Find(customerId);
            if (customerToFind == null || currentUser == null)
            {
                return NotFound();
            }
            if (!(customerToFind == currentUser))
            {
                return Unauthorized();
            }

            var orders = _context.Orders.Where(x => x.User == customerToFind);

            return Ok(orders);
        }

        // <baseurl>/api/order/date
        // Only available to admins.
        [HttpGet("date"), Authorize(Roles = "Admin")]
        public IActionResult GetOrdersByDate([FromBody] DateDto data)
        {
            DateTime usableDate = new DateTime(data.Year, data.Month, data.Day, data.Hours, data.Minutes, data.Seconds);
            var orders = _context.Orders.Where(x => x.Date == usableDate);

            return Ok(orders);
        }

        // <baseurl>/api/order
        // Only available to buyers.
        [HttpPut]
        public IActionResult UpdateOrder([FromBody] Order value)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            Order orderToChange = _context.Orders.Find(value.OrderId);
            if (orderToChange == null || currentUser == null)
            {
                return NotFound();
            }
            if (orderToChange.User != currentUser)
            {
                return Unauthorized();
            }
            orderToChange.UserId = value.UserId;
            orderToChange.Date = value.Date;
            _context.Update(orderToChange);
            _context.SaveChanges();
            return Ok(orderToChange);
        }

        // <baseurl>/api/order
        // Available to any authorized user.
        [HttpPost, Authorize]
        public IActionResult NewOrder([FromBody] OrderDto value)
        {
            DateTime usableDate = new DateTime(value.Year, value.Month, value.Day, value.Hours, value.Minutes, value.Seconds);
            Order newOrder = new Order() { UserId = value.UserId, Date = usableDate };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            return StatusCode(201, newOrder);
        }

        // <baseurl>/api/order/<id>
        // Only available to buyers.
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var orderToDelete = _context.Orders.Find(id);
            if (orderToDelete == null || currentUser == null)
            {
                return NotFound();
            }
            if (orderToDelete.User != currentUser)
            {
                return Unauthorized();
            }
            _context.Orders.Remove(orderToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}