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
    public class ProductOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllProductOrders()
        {
            var productOrders = _context.ProductOrders;
            return Ok(productOrders);
        }

        // <baseurl>/api/examples/user
        [HttpGet("{id}")]
        public IActionResult GetSelectedProductOrder(int id)
        {
            var productOrder = _context.ProductOrders.Find(id);
            if (productOrder == null)
            {
                return NotFound();
            }
            return Ok(productOrder);
        }

        [HttpGet("order/{orderId}")]
        public IActionResult GetProductOrdersByOrder(int orderId)
        {
            var orderToFind = _context.Orders.Find(orderId);
            var productOrders = _context.ProductOrders.Where(x => x.Order == orderToFind);

            return Ok(productOrders);
        }

        [HttpPost]
        public IActionResult NewProductOrder([FromBody] ProductOrder value)
        {
            _context.ProductOrders.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpPut]
        public IActionResult UpdateProductOrder([FromBody] ProductOrder value)
        {
            ProductOrder productOrderToChange = _context.ProductOrders.Find(value.ProductOrderId);
            if (productOrderToChange != null)
            {
                return NotFound();
            }
            productOrderToChange.ProductId = value.ProductId;
            productOrderToChange.OrderId = value.OrderId;
            productOrderToChange.Quantity = value.Quantity;
            _context.Update(productOrderToChange);
            _context.SaveChanges();
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProductOrder(int id)
        {
            var productOrderToDelete = _context.ProductOrders.Find(id);
            if (productOrderToDelete == null)
            {
                return NotFound();
            }
            _context.ProductOrders.Remove(productOrderToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}