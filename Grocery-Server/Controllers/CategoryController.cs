using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly DbContext _dbContext;

        public CategoryController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("all")]
        public IActionResult GetCategories([FromQuery] Guid householdId)
        {
            HouseHold? household = _dbContext.HouseHolds.FirstOrDefault(hh => hh.Id == householdId);
            if (household == null)
                return NotFound();
            _dbContext.Entry(household).Collection(household => household.CustomCategories).Load();
            List<CategoryDisplayDTO> categories = household.CustomCategories.Select(cat => new CategoryDisplayDTO(cat)).ToList();
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewCategoryDTO newCategory)
        {
            if (!_dbContext.HouseHolds.Any(hh => hh.Id == newCategory.HouseHoldId))
                return NotFound();

            GroceryCategory category = new(newCategory);
            _dbContext.GroceryCategories.Add(category);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] Guid id)
        {
            GroceryCategory? category = _dbContext.GroceryCategories.FirstOrDefault(cat => cat.Id == id);
            if (category == null)
                return NotFound();

            _dbContext.GroceryCategories.Remove(category);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
