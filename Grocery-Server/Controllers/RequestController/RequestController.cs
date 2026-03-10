using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Grocery_Server.Controllers.RequestController;

[ApiController]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[RequireGroup]
[Route("api/requests")]
public class RequestController : ControllerBase
{
    private readonly GroceryDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public RequestController(GroceryDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IEnumerable<RequestedItemDTO>> GetRequests()
    {
        // safe to assert group != null due to RequireGroup attr on controller
        return (await GetUser()).Group!.RequestListItems.Select(rli => new RequestedItemDTO(rli.ItemId, rli.Item.ItemName, rli.Quantity));
    }

    [HttpPost]
    public async Task<IActionResult> RequestItem([FromBody] RequestListItemDTO newItem)
    {
        var myGroup = (await GetUser()).Group!;
        var item = _dbContext.GroceryItems.FirstOrDefault(i => i.Id == newItem.ItemId);
        if (item == null)
            return NotFound("No such item was found");

        var foundItem = myGroup.RequestListItems.FirstOrDefault(rli => rli.ItemId == newItem.ItemId);
        if (foundItem != null)
            foundItem.Quantity = newItem.Quantity;
        else
            myGroup.RequestListItems.Add(new RequestListItem(myGroup.Id, newItem.ItemId, newItem.Quantity));
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> UnrequestItem(Guid id)
    {
        var myGroup = (await GetUser()).Group!;
        var foundItem = myGroup.RequestListItems.FirstOrDefault(rli => rli.ItemId == id);
        if (foundItem == null)
            return NotFound("Item was not requested, cannot remove.");
        _dbContext.Remove(foundItem);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("reset")]
    public async Task<IActionResult> ResetList()
    {
        var myGroup = (await GetUser()).Group!;
        myGroup.RequestListItems.Clear();
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [NonAction]
    private async Task<User> GetUser()
    {
        return await _userManager.GetUserAsync(User) ?? throw new Exception("Couldn't get user??");
    }
}
