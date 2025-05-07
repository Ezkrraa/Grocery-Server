//using Grocery_Server.ControllerModels;
using Grocery_Server.Controllers.Category;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Grocery_Server.Controllers.ItemController;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/item")]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
[RequireGroup]
public class ItemController : ControllerBase
{
    private readonly GroceryDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public ItemController([FromServices] GroceryDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] NewItemDTO itemDTO)
    {
        if (itemDTO.Name.Trim().IsNullOrEmpty())
            return BadRequest();

        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        GroceryCategory? category = group.CustomCategories.FirstOrDefault(cat =>
            cat.Id == itemDTO.CategoryId
        );
        if (category == null)
            return BadRequest();

        if (category.Items.Any(item => item.ItemName == itemDTO.Name))
            return Conflict("Item already exists in category");

        GroceryItem newItem = new(itemDTO);
        newItem.ItemName = newItem.ItemName.Trim().ToLower();
        newItem.ItemName = char.ToUpper(newItem.ItemName[0]) + newItem.ItemName[1..];
        _dbContext.GroceryItems.Add(newItem);
        await _dbContext.SaveChangesAsync();
        return Ok(newItem.Id);
    }

    [HttpGet("by-group")]
    public async Task<IActionResult> GetItemsFromGroup()
    {
        User user = await GetCurrentUser();

        return Ok(
            GetGroup(user).CustomCategories.Select(category => new CategoryListDTO(category))
        );
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
            return NotFound();
        return Ok(new ItemDetailDisplayDTO(foundItem));
    }

    [HttpGet("by-category")]
    public async Task<IActionResult> GetItemsInCategory([FromQuery] Guid id)
    {
        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        GroceryCategory? category = group.CustomCategories.FirstOrDefault(category =>
            category.Id == id
        );
        if (category == null)
            return NotFound();
        return Ok(new CategoryListDTO(category));
    }

    [EnableRateLimiting(nameof(RateLimiters.Slow))]
    [HttpPatch("rename")]
    public async Task<IActionResult> RenameItem([FromBody] RenameCategoryDTO renamedItem)
    {
        GroceryItem? toRenameItem = _dbContext.GroceryItems.FirstOrDefault(item =>
            item.Id == renamedItem.Id
        );
        if (toRenameItem == null)
            return NotFound();
        toRenameItem.ItemName = renamedItem.Name;
        await _dbContext.SaveChangesAsync();
        return Ok(new ItemDetailDisplayDTO(toRenameItem));
    }

    [EnableRateLimiting(nameof(RateLimiters.Slow))]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
    {
        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        GroceryItem? existingItem = _dbContext.GroceryItems.FirstOrDefault(item =>
            item.Id == id
        );
        if (existingItem == null || existingItem.Category.GroupId != group.Id)
            return NotFound();
        _dbContext.GroceryItems.Remove(existingItem);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchItem(string name)
    {
        User user = await GetCurrentUser();
        Group group = GetGroup(user);

        List<ItemDetailDisplayDTO> items = await _dbContext.GroceryItems
            .Where(item => item.ItemName.ToLower().Contains(name.ToLower()))
            .Where(item => item.Category.GroupId == group.Id)
            .Select(item => new ItemDetailDisplayDTO(item))
            .ToListAsync();
        return Ok(items);
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
