using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Server.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/item")]
    [RequireGroup]
    public class ItemController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ItemController([FromServices] DbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] NewItemDTO itemDTO)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryCategory? category = group.CustomCategories.FirstOrDefault(cat => cat.Id == itemDTO.CategoryId);
            if (category == null)
                return BadRequest();

            if (category.Items.Any(item => item.ItemName == itemDTO.Name))
                return Conflict("Item already exists in category");

            GroceryItem newItem = new(itemDTO);
            _dbContext.GroceryItems.Add(newItem);
            _dbContext.SaveChanges();
            return Ok(new ItemDetailDisplayDTO(newItem));
        }

        [HttpGet("by-group")]
        public async Task<IActionResult> GetItemsFromGroup()
        {
            User user = await GetCurrentUser();

            return Ok(GetGroup(user).CustomCategories.Select(category => new CategoryListDTO(category)));
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetItemById([FromQuery] Guid id)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryItem? foundItem = _dbContext.GroceryItems.FirstOrDefault(item => item.Id == id);
            if (foundItem == null)
                return NotFound();
            if (foundItem.Category.Group != group) // act like item does not exist if unowned
                return Ok();
            return Ok(new ItemDetailDisplayDTO(foundItem));
        }

        [HttpGet("by-category")]
        public async Task<IActionResult> GetItemsInCategory([FromQuery] Guid id)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryCategory? category = group.CustomCategories.FirstOrDefault(category => category.Id == id);
            if (category == null)
                return NotFound();
            return Ok(category.Items.Select(item => new ItemDisplayDTO(item)));
        }

        [HttpPatch("rename")]
        public IActionResult RenameItem([FromBody] RenameInfoDTO renamedItem)
        {
            GroceryItem? toRenameItem = _dbContext.GroceryItems.FirstOrDefault(item => item.Id == renamedItem.Id);
            if (toRenameItem == null)
                return NotFound();
            toRenameItem.ItemName = renamedItem.Name;
            _dbContext.SaveChanges();
            return Ok(new ItemDetailDisplayDTO(toRenameItem));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryItem? existingItem = _dbContext.GroceryItems.FirstOrDefault(item => item.Id == id);
            if (existingItem == null || existingItem.Category.GroupId != group.Id)
                return NotFound();
            _dbContext.GroceryItems.Remove(existingItem);
            _dbContext.SaveChanges();
            return Ok();
        }


        private async Task<User> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User) ?? throw new Exception("Should be impossible");
        }

        private Group GetGroup(User user)
        {
            return user.Group ?? throw new Exception("Should never happen");
        }
    }
}
