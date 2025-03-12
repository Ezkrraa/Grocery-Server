//using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Grocery_Server.Controllers.GroceryListController;

[ApiController]
[Route("api/grocery-list")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
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

    [HttpGet("get-list/{id}")]
    public async Task<IActionResult> GetList(Guid id)
    {
        User user = await GetCurrentUser();

        Models.GroceryList? list = user.Group?.GroceryLists?.FirstOrDefault(list =>
            list.Id == id
        );

        if (list == null || user.GroupId != list.GroupId)
            return NotFound();
        List<GroceryListItem> items = list.GroceryListItems?.ToList();
        return Ok(
            list.GroceryListItems?.Select(item => new ListItemDisplayDTO(
                item.ItemId,
                item.Item.ItemName,
                item.Quantity,
                item.Item.CategoryId,
                item.Item.Category.CategoryName
            ))
        );
    }

    [EnableRateLimiting(nameof(RateLimiters.Slow))]
    [HttpPost("create-list")]
    public async Task<IActionResult> CreateList([FromBody] CreateListDTO itemsList)
    {
        User user = await GetCurrentUser();

        Grocery_Server.Models.GroceryList newList = new(itemsList, user.Group);

        if (
            !itemsList.Items.All(item =>
                _dbContext.GroceryItems.Any(existingItem =>
                    existingItem.Id == item.ItemId
                    && existingItem.Category.GroupId == user.GroupId
                )
            )
        )
            return BadRequest();

        _dbContext.GroceryLists.Add(newList);

        newList.GroceryListItems = itemsList
            .Items.Select(item => new GroceryListItem(newList.Id, item.ItemId, item.Quantity))
            .ToList();
        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpGet("get-all-lists")]
    public async Task<IActionResult> GetAllLists()
    {
        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        return Ok(group.GroceryLists?.Select(list => new GroceryListDisplayDTO(list)));
    }

    [EnableRateLimiting(nameof(RateLimiters.Slow))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteList(Guid id)
    {
        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        GroceryList? foundList = _dbContext.GroceryLists.FirstOrDefault(list => list.Id == id);
        if (foundList == null || foundList.Group != group)
            return NotFound();

        if (foundList.GroceryListItems != null)
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

        GroceryList? foundList = _dbContext.GroceryLists.FirstOrDefault(list =>
            list.Id == newItem.ListId
        );
        if (foundList == null || foundList.Group != group)
            return NotFound();

        GroceryItem? foundItem = _dbContext.GroceryItems.FirstOrDefault(item =>
            item.Id == newItem.ItemId
        );
        if (foundItem == null || foundItem.Category.Group != group)
            return NotFound();

        if (foundList.GroceryListItems.Any(item => item.ItemId == newItem.ItemId))
            return Conflict();

        foundList.GroceryListItems.Add(
            new GroceryListItem(foundList.Id, newItem.ItemId, newItem.Quantity)
        );
        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpPatch("remove")]
    public async Task<IActionResult> RemoveFromList([FromBody] AddToListItemDTO toRemoveItem)
    {
        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        GroceryList? foundList = _dbContext.GroceryLists.FirstOrDefault(list =>
            list.Id == toRemoveItem.ListId
        );
        if (foundList == null || foundList.Group != group || foundList.GroceryListItems == null)
            return NotFound();

        GroceryListItem? item = foundList.GroceryListItems.FirstOrDefault(item =>
            item.ItemId == toRemoveItem.ItemId
        );
        if (item == null)
            return NotFound();

        _dbContext.Remove(item);
        _dbContext.SaveChanges();
        return Ok();
    }

    private async Task<User> GetCurrentUser()
    {
        return await _userManager.GetUserAsync(User)
            ?? throw new Exception("Should be impossible");
    }

    private Group GetGroup(User user)
    {
        return user.Group ?? throw new Exception("Should never happen");
    }
}
