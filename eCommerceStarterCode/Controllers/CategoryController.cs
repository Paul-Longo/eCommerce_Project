using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Mvc;


namespace eCommerceStarterCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // <baseurl>/api/categories
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _context.Categories;
            return Ok(categories);
        }

        // <baseurl>/api/category/1
        [HttpGet("{id}")]
        public IActionResult GetCategoryId(int id)
        {
            var categories = _context.Categories;
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult NewCategory([FromBody] Category value)
        {
            _context.Categories.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryToDelete = _context.Categories.Find(id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }

}