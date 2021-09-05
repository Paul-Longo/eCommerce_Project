using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var reviews = _context.Reviews;
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reviews = _context.Reviews.Include(r => r.Product).Include(r => r.User).Where(review => review.ProductId == id);
            return Ok(reviews);
        }

        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Review value)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);

            _context.Reviews.Add(value);
            var product = _context.Products.FirstOrDefault(product => product.ProductId == value.ProductId);
            _context.SaveChanges();
            return Ok(value);
        }

        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] Review value)
        {
            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var review = _context.Reviews.FirstOrDefault(review => review.ReviewId == id);

            review.Body = value.Body;
            review.Rating = value.Rating;
            review.UserId = value.UserId;
            _context.SaveChanges();
            return Ok(review);
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {

            string userId = User.FindFirstValue("id");
            User currentUser = _context.Users.Find(userId);
            var review = _context.Reviews.FirstOrDefault(review => review.ReviewId == id);

            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return Ok();
        }
    }
}

