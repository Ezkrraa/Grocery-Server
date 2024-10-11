using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net;

namespace Grocery_Server.Controllers
{
    //TODO: add authorization
    // when implementing auth, remember to update existing methods to prevent
    // unauthorized CRUD on non-owned items
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly DbContext _dbContext;
        public ItemController([FromServices] DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult CreateItem([FromBody] NewItemDTO itemDTO)
        {
            if (!_dbContext.GroceryCategories.Any(category => category.Id == itemDTO.CategoryId))
                return BadRequest();
            GroceryItem newItem = new(itemDTO);
            _dbContext.GroceryItems.Add(newItem);
            _dbContext.SaveChanges();
            return Ok(newItem);
        }

        [HttpGet("by-id")]
        public IActionResult GetItemById([FromQuery] Guid Id)
        {
            return Ok(_dbContext.GroceryItems.FirstOrDefault(item => item.Id == Id));
        }

        [HttpGet("by-category")]
        public IActionResult GetItemsInCategory([FromQuery] Guid CategoryId)
        {
            GroceryCategory? category = _dbContext.GroceryCategories.FirstOrDefault(category => category.Id == CategoryId);
            if (category == null)
                return NotFound();
            _dbContext.Entry(category).Collection(category => category.Items).Load();
            return Ok(category.Items.Select(item => new ItemDisplayDTO(item)));
        }

        [HttpPatch("rename")]
        public IActionResult RenameItem([FromBody] RenameItemDTO renamedItem)
        {
            GroceryItem? toRenameItem = _dbContext.GroceryItems.FirstOrDefault(item => item.Id == renamedItem.Id);
            if (toRenameItem == null)
                return NotFound();
            toRenameItem.ItemName = renamedItem.Name;
            _dbContext.SaveChanges();
            return Ok(toRenameItem);
        }

        [HttpDelete("delete")]
        public IActionResult DeleteItem([FromQuery] Guid id)
        {
            GroceryItem? existingItem = _dbContext.GroceryItems.FirstOrDefault(item => item.Id == id);
            if (existingItem == null)
                return NotFound();
            _dbContext.GroceryItems.Remove(existingItem);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
