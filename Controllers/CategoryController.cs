using BulkyBook.Dependencies;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Controllers;
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoryController : Controller
{
   private readonly ApplicationDbContext _context;
   private readonly ILogger<CategoryController> _logger;
   private readonly Dictionary<int, IMyDependency> handlers;

        public CategoryController(ApplicationDbContext context,ILogger<CategoryController> logger,IEnumerable<IMyDependency> myDep)
        {
            _context = context;
            _logger = logger;
            int c = 0;
            this.handlers = myDep.ToDictionary(
                (handler) => c++,
                handler => handler
            );
        }

        // GET: api/category
        [HttpGet]
        [Authorize(Policy = "adham")]
        public IActionResult GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            this.handlers.TryGetValue(0, out IMyDependency _myDep);
            if (_myDep == null)
                return Ok(0);
            _myDep.WriteMessage("adham");
            this.handlers.TryGetValue(1, out IMyDependency _myDep2);
            if (_myDep2 == null)
                return Ok(1);
            _myDep.WriteMessage("adham");
            _logger.LogInformation("hello zizo");
            return Ok(new Dictionary<string, object>()
                {
                    { "name", _myDep.Name },
                    { "name2", _myDep2.Name },
                    { "categories", categories },
                }
            );
        }

        // GET: api/categories/1
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public IActionResult PostCategory([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                // Set CreatedDateTime to the current time (DateTime.Now is UTC by default)
                category.CreatedDateTime = DateTime.Now;

                // Add the category to the database
                _context.Categories.Add(category);
                _context.SaveChanges();

                // Return the newly created category as JSON in the response
                return Ok(category);
            }
            else
            {
                // If the model state is not valid, return a bad request with the validation errors
                return BadRequest(ModelState);
            }
        }

        // PUT: api/categories/1
        [HttpPut("{id}")]
        public IActionResult PutCategory(int id, [FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                Category? existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

                if (existingCategory == null)
                {
                    return NotFound();
                }

                // Update properties of the existing category
                existingCategory.Name = category.Name;
                existingCategory.DisplayOrder = category.DisplayOrder;

                // Save the changes to the database
                _context.SaveChanges();

                // Return the updated category as JSON in the response
                return Ok(existingCategory);
            }
            else
            {
                // If the model state is not valid, return a bad request with the validation errors
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/categories/1
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            // Remove the category from the database
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }
}