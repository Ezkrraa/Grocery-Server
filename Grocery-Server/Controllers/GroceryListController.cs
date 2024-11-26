using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Route("api/grocery-list-controller")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RequireGroup]
    public class GroceryListController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public GroceryListController(DbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetList([FromQuery] Guid id)
        {
            User user = await GetCurrentUser();

            Models.GroceryList? list = user.Group.GroceryLists.FirstOrDefault(list => list.Id == id);

            if (list == null || user.GroupId != list.GroupId)
                return NotFound();
            List<Models.GroceryListItem> items = list.GroceryListItems.ToList();
            return Ok(list.GroceryListItems.Select(item => new ListItemDisplayDTO(item.ItemId, item.Item.ItemName, item.Quantity, item.Item.CategoryId)));
        }

        [HttpPost("create-list")]
        public async Task<IActionResult> CreateList([FromBody] CreateListDTO itemsList)
        {
            User user = await GetCurrentUser();

            GroceryList newList = new(itemsList, user.Group);

            if (!itemsList.Items.All(item => _dbContext.GroceryItems.Any(existingItem => existingItem.Id == item.ItemId && existingItem.Category.GroupId == user.GroupId)))
                return BadRequest();

            _dbContext.GroceryLists.Add(newList);

            newList.GroceryListItems = itemsList.Items
                .Select(item => new GroceryListItem(newList.Id, item.ItemId, item.Quantity))
                .ToList();
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("get-all-lists")]
        public async Task<IActionResult> GetAllLists()
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            return Ok(group.GroceryLists.Select(list => new GroceryListDisplayDTO(list)));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteList([FromBody] Guid id)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryList? foundList = _dbContext.GroceryLists.FirstOrDefault(list => list.Id == id);
            if (foundList == null || foundList.Group != group)
                return NotFound();

            _dbContext.RemoveRange(foundList.GroceryListItems);
            _dbContext.Remove(foundList);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPatch("add")]
        public async Task<IActionResult> AddToList([FromBody] AddToListItemDTO newItem)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryList? foundList = _dbContext.GroceryLists.FirstOrDefault(list => list.Id == newItem.ListId);
            if (foundList == null || foundList.Group != group)
                return NotFound();

            GroceryItem? foundItem = _dbContext.GroceryItems.FirstOrDefault(item => item.Id == newItem.ItemId);
            if (foundItem == null || foundItem.Category.Group != group)
                return NotFound();

            foundList.GroceryListItems.Add(new GroceryListItem(foundList.Id, newItem.ItemId, newItem.Quantity));
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPatch("remove")]
        public async Task<IActionResult> RemoveFromList([FromBody] AddToListItemDTO toRemoveItem)
        {
            User user = await GetCurrentUser();
            Group group = GetGroup(user);

            GroceryList? foundList = _dbContext.GroceryLists.FirstOrDefault(list => list.Id == toRemoveItem.ListId);
            if (foundList == null || foundList.Group != group)
                return NotFound();

            GroceryListItem? item = foundList.GroceryListItems.FirstOrDefault(item => item.ItemId == toRemoveItem.ItemId);
            if (item == null)
                return NotFound();

            _dbContext.Remove(item);
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
